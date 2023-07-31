<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" Async="true" 
    CodeFile="~/Common/CorporateOffice.aspx.cs" Inherits="Common_CorporateOffice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

      
            <script type="text/javascript">
                function CorLogoPrev(input) {
                    var fup = document.getElementById("<%=fupCorLogo.ClientID %>");
                    var fileName = fup.value;
                    var maxfilesize = 1024 * 1024;
                    filesize = input.files[0].size;
                    var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
                    if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" || ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG" || ext == "svg" || ext == "SVG") {
                        if (filesize <= maxfilesize) {
                            if (input.files && input.files[0]) {
                                var reader = new FileReader();
                                reader.onload = function (e) {
                                    $('#<%=imgCorLogoPrev.ClientID%>').prop('src', e.target.result);
                                    $('#<%=hfCorLogoChkVal.ClientID%>').val("1");
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
                function CorLogoPrev1(input) {
                    var fup1 = document.getElementById("<%=fupCorLogo1.ClientID %>");
                    var fileName1 = fup1.value;
                    var maxfilesize1 = 1024 * 1024;
                    filesize1 = input.files[0].size;
                    var ext1 = fileName1.substring(fileName1.lastIndexOf('.') + 1);
                    if (ext1 == "gif" || ext1 == "GIF" || ext1 == "PNG" || ext1 == "png" || ext1 == "jpg" || ext1 == "JPG" || ext1 == "bmp" || ext1 == "BMP" || ext1 == "jpeg" || ext1 == "JPEG" || ext1 == "svg" || ext1 == "SVG") {
                        if (filesize1 <= maxfilesize1) {
                            if (input.files && input.files[0]) {
                                var reader = new FileReader();
                                reader.onload = function (e) {
                                    $('#<%=imgCorLogo1Prev.ClientID%>').prop('src', e.target.result);
                                    $('#<%=hfCorLogo1ChkVal.ClientID%>').val("1");
                                };
                                reader.readAsDataURL(input.files[0]);
                            }
                        }
                        else {
                            swal("Please, Upload image file less than or equal to 1 MB !!!");
                            fup1.focus();
                            return false;
                        }
                    }
                    else {
                        swal("Please, Upload Gif, Jpg, Jpeg, Svg or Bmp Images only !!!");
                        fup1.focus();
                        return false;
                    }
                }
                function CorPhotoPrev(input) {
                    var fup2 = document.getElementById("<%=fupCorPhoto.ClientID %>");
                    var fileName2 = fup2.value;
                    var maxfilesize2 = 1024 * 1024;
                    filesize2 = input.files[0].size;
                    var ext2 = fileName2.substring(fileName2.lastIndexOf('.') + 1);
                    if (ext2 == "gif" || ext2 == "GIF" || ext2 == "PNG" || ext2 == "png" || ext2 == "jpg" || ext2 == "JPG" || ext2 == "bmp" || ext2 == "BMP" || ext2 == "jpeg" || ext2 == "JPEG" || ext2 == "svg" || ext2 == "SVG") {
                        if (filesize2 <= maxfilesize2) {
                            if (input.files && input.files[0]) {
                                var reader = new FileReader();
                                reader.onload = function (e) {
                                    $('#<%=imgCorPhotoPrev.ClientID%>').prop('src', e.target.result);
                                    $('#<%=hfCorPhotoChkVal.ClientID%>').val("1");
                                };
                                reader.readAsDataURL(input.files[0]);
                            }
                        }
                        else {
                            swal("Please, Upload image file less than or equal to 1 MB !!!");
                            fup2.focus();
                            return false;
                        }
                    }
                    else {
                        swal("Please, Upload Gif, Jpg, Jpeg, Svg or Bmp Images only !!!");
                        fup2.focus();
                        return false;
                    }
                }
                function CorPhotoPrev1(input) {
                    var fup3 = document.getElementById("<%=fupCorPhoto1.ClientID %>");
                    var fileName3 = fup3.value;
                    var maxfilesize3 = 1024 * 1024;
                    filesize3 = input.files[0].size;
                    var ext3 = fileName3.substring(fileName3.lastIndexOf('.') + 1);
                    if (ext3 == "gif" || ext3 == "GIF" || ext3 == "PNG" || ext3 == "png" || ext3 == "jpg" || ext3 == "JPG" || ext3 == "bmp" || ext3 == "BMP" || ext3 == "jpeg" || ext3 == "JPEG" || ext3 == "svg" || ext3 == "SVG") {
                        if (filesize3 <= maxfilesize3) {
                            if (input.files && input.files[0]) {
                                var reader = new FileReader();
                                reader.onload = function (e) {
                                    $('#<%=imgCorPhoto1Prev.ClientID%>').prop('src', e.target.result);
                                    $('#<%=hfCorPhoto1ChkVal.ClientID%>').val("1");
                                };
                                reader.readAsDataURL(input.files[0]);
                            }
                        }
                        else {
                            swal("Please, Upload image file less than or equal to 1 MB !!!");
                            fup3.focus();
                            return false;
                        }
                    }
                    else {
                        swal("Please, Upload Gif, Jpg, Jpeg, Svg or Bmp Images only !!!");
                        fup3.focus();
                        return false;
                    }
                }
            </script>
            <script type="text/javascript">

                $(function () {
                    var fileupload = $('#<%=fupCorLogo.ClientID%>');
                    var image = $('#divImgPre1');
                    image.click(function () {
                        fileupload.click();
                    });
                });
                $(function () {
                    var fileupload = $('#<%=fupCorLogo1.ClientID%>');
                    var image = $('#divImgPre2');
                    image.click(function () {
                        fileupload.click();
                    });
                });

                $(function () {
                    var fileupload = $('#<%=fupCorPhoto.ClientID%>');
                    var image = $('#divImgPre3');
                    image.click(function () {
                        fileupload.click();
                    });
                });
                $(function () {
                    var fileupload = $('#<%=fupCorPhoto1.ClientID%>');
                    var image = $('#divImgPre4');
                    image.click(function () {
                        fileupload.click();
                    });
                });

                //Script For Add & Minus For Logo

                $(function () {
                    let imageToggle = $('#<%=hfCorLoDisp.ClientID%>');


                    if (imageToggle.val() === "1") {
                        $("#divCorLogoNew").show();
                    }
                    else {
                        $("#divCorLogoNew").hide();
                    }
                })
                $(function () {
                    let imageToggle1 = $('#<%=hfCorPhDisp.ClientID%>');


                    if (imageToggle1.val() === "1") {
                        $("#divCorPhotoNew").show();
                    }
                    else {
                        $("#divCorPhotoNew").hide();
                    }
                })

                $(function () {
                    $('#ibtncorporateLogoAdd').on("click", function () {
                        $("#divCorLogoNew").show();
                    })
                });

                $(function () {
                    $('#ibtncorporateLogoMinus').on("click", function () {
                        $('#<%=imgCorLogo1Prev.ClientID%>').attr("src", "images/FileUpload.png");
                        $("#divCorLogoNew").hide();
                        $('#<%=hfCorLoMinus.ClientID%>').val("1");
                    })
                });

                //Script For Add & Minus For Photo

                $(function () {
                    $('#ibtnCorporationphotoadd').on("click", function () {
                        $("#divCorPhotoNew").show();
                    })
                });

                $(function () {
                    $('#ibtnCorporationMinus').on("click", function () {
                        $('#<%=imgCorPhoto1Prev.ClientID%>').attr("src", "images/FileUpload.png");
                        $("#divCorPhotoNew").hide();
                        $('#<%=hfCorPhMinus.ClientID%>').val("1");
                    })
                });

            </script>
            <style>
                #map {
                    height: 590px;
                    width: 400px;
                    border: groove;
                    color: black;
                }
            </style>

            <div class="form-body">
                <h5 class="pghr">Corporate Office <span style="float: right;">
                    <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
                <hr />
                <div class="form-body" id="divEntry" runat="server" visible="false" >
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-sm-9">
                                <div class="row">
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" visible="false">
                                        <div class="form-group">
                                            <label runat="server" id="Label3"><i class="fa fa-ship" aria-hidden="true"></i>CropID</label>
                                            <asp:TextBox ID="txtCropId" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                                MaxLength="2" Font-Size="14px" TabIndex="1">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label4"><i class="fa fa-landmark" aria-hidden="true"></i>Corporation Name <span class="spStar">*</span></label>
                                            <asp:TextBox ID="txtCropName" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                                MaxLength="100" Font-Size="14px" TabIndex="2">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCropName"
                                                ValidationGroup="CompanyMaster" SetFocusOnError="True" CssClass="vError">Enter Corporation Name</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label1"><i class="fa fa-map-marker" aria-hidden="true"></i>Short Name</label>
                                            <asp:TextBox ID="txtshortName" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                                MaxLength="10" Font-Size="14px" TabIndex="3">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtshortName"
                                                ValidationGroup="CompanyMaster" SetFocusOnError="True" CssClass="vError">Enter Short Name</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" class="boataddress"><i class="fas fa-map-pin"></i>Zip Code<span class="spStar">*</span> </label>
                                            <asp:TextBox ID="txtZipcode" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off" OnTextChanged="txtZipcode_TextChanged"
                                                MaxLength="6" Font-Size="14px" TabIndex="4" onkeypress="return isNumber(event)" AutoPostBack="true">
                                            </asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtZipcode"
                                                ValidationGroup="BoatHouseName" SetFocusOnError="True" CssClass="vError">Enter Zip Code</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" runat="server" visible="false" id="ZipcodeEntry">
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" class="boataddress"><i class="fas fa-city"></i>City </label>
                                            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control inputboxstyle" ReadOnly="true"
                                                MaxLength="100" Font-Size="14px" TabIndex="5">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" class="boataddress"><i class="fas fa-map-marker"></i>District </label>
                                            <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control inputboxstyle" ReadOnly="true"
                                                MaxLength="50" Font-Size="14px" TabIndex="6">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" class="boataddress"><i class="fas fa-map-marker"></i>State </label>
                                            <asp:TextBox ID="txtState" runat="server" CssClass="form-control inputboxstyle" ReadOnly="true"
                                                MaxLength="50" Font-Size="14px" TabIndex="7">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" runat="server" visible="false" id="Address">
                                    <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" class="boataddress"><i class="fa fa-home"></i>Door/FlatNo, StreetName</label>
                                            <asp:TextBox ID="txtAddress1" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                                MaxLength="100" Font-Size="14px" TabIndex="8">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-4 col-md-4 col-lg-4 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" class="boataddress"><i class="fas fa-store-alt"></i>Location / Area Name</label>
                                            <asp:TextBox ID="txtaddress2" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto" Placeholder=""
                                                MaxLength="100" Font-Size="14px" TabIndex="9">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" runat="server" visible="false" id="PhoneNumber">
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label2"><i class="fa fa-map-marker" aria-hidden="true"></i>Country <span class="spStar">*</span></label>
                                            <asp:DropDownList ID="ddlCountry" CssClass="form-control inputboxstyle" runat="server" TabIndex="10">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlCountry"
                                                ValidationGroup="CompanyMaster" InitialValue="Select Country" SetFocusOnError="True" CssClass="vError">Select Country</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label8"><i class="fa fa-phone" aria-hidden="true"></i>Phone1</label>
                                            <asp:TextBox ID="txtphone1" runat="server" CssClass="form-control inputboxstyle" onkeypress="return isNumber(event);" AutoComplete="Off"
                                                MaxLength="10" Font-Size="14px" TabIndex="11">
                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                                ControlToValidate="txtphone1" ErrorMessage="Invalid"
                                                ValidationExpression="[0-9]{10}" ForeColor="Red"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label9"><i class="fa fa-phone" aria-hidden="true"></i>Phone2</label>
                                            <asp:TextBox ID="txtphone2" runat="server" CssClass="form-control inputboxstyle" onkeypress="return isNumber(event);" AutoComplete="Off"
                                                MaxLength="10" Font-Size="14px" TabIndex="12">
                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                                ControlToValidate="txtphone2" ErrorMessage="Invalid"
                                                ValidationExpression="[0-9]{10}" ForeColor="Red"></asp:RegularExpressionValidator>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" runat="server" visible="false" id="FaxEmail">
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label7"><i class="fa fa fa-fax" aria-hidden="true"></i>Fax</label>
                                            <asp:TextBox ID="txtFax" runat="server" CssClass="form-control inputboxstyle" onkeypress="return isNumber(event);" AutoComplete="Off"
                                                MaxLength="10" Font-Size="14px" TabIndex="13">
                                            </asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label for="lblBkfrm" id="Label6" runat="server">
                                                <i class="fa fa-calendar" aria-hidden="true"></i>Email Id</label>
                                            <div id="Div2" runat="server">
                                                <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" AutoComplete="Off" TabIndex="14">
                                                </asp:TextBox>
                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtEmailId"
                                                    SetFocusOnError="True" CssClass="vError" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                                    Display="Dynamic" ErrorMessage="Invalid" ValidationGroup="OfferMaster" />
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label>Application Name <span class="spStar">*</span></label>
                                            <asp:TextBox ID="txtappname" runat="server" CssClass="form-control inputboxstyle" AutoComplete="Off"
                                                MaxLength="10" Font-Size="14px" TabIndex="15">
                                            </asp:TextBox>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtappname" ForeColor="Red"
                                                ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Invalid" CssClass="vError" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtappname"
                                                ValidationGroup="CompanyMaster" SetFocusOnError="True" CssClass="vError">Enter Application Name</asp:RequiredFieldValidator>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" runat="server" visible="false" id="CorporationLogo">
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label13">Corporation Logo <span class="spStar">*</span></label>
                                            &nbsp; &nbsp; &nbsp;
                                    <img id="ibtncorporateLogoAdd" src="Images/Add.png" alt="" width="20" class="imgOutLine" />
                                            <br />
                                            <span id="spnFilePathlogo"></span>
                                            <asp:FileUpload ID="fupCorLogo" runat="server" Style="display: none" onchange="CorLogoPrev(this);" />

                                            <div class="divImg" id="divImgPre1">
                                                <asp:Image ID="imgCorLogoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                                <div class="imageOverlay">Click To Upload</div>
                                            </div>

                                        </div>
                                    </div>
                                     <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label12">Corporation Photo <span class="spStar">*</span></label>
                                            &nbsp  &nbsp &nbsp 
                                     <img id="ibtnCorporationphotoadd" src="Images/Add.png" alt="" width="20" class="imgOutLine" />
                                            <br />
                                            <span id="spnFilePath"></span>
                                            <asp:FileUpload ID="fupCorPhoto" runat="server" Style="display: none" onchange="CorPhotoPrev(this);" />
                                            <div class="divImg" id="divImgPre3">
                                                <asp:Image ID="imgCorPhotoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                                <div class="imageOverlay">Click To Upload</div>
                                            </div>

                                        </div>
                                    </div>
                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" style="display: none" id="divCorLogoNew">
                                        <div class="form-group">
                                            <label runat="server" id="Label10">Corporation Logo 1</label>
                                            &nbsp &nbsp &nbsp
                                    <img id="ibtncorporateLogoMinus" src="Images/Minus.png" alt="" width="20" class="imgOutLine" />
                                            <br />
                                            <span id="spnFilePathlogo1"></span>
                                            <asp:FileUpload ID="fupCorLogo1" runat="server" Style="display: none" onchange="CorLogoPrev1(this);" />
                                            <div class="divImg" id="divImgPre2">
                                                <asp:Image ID="imgCorLogo1Prev" runat="server" alt="Select File" title="Select File" ImageUrl="~/Images/FileUpload.png" Width="100%" />
                                                <div class="imageOverlay">Click To Upload</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row" runat="server" visible="false" id="CorporationPhoto12">
                                   <%-- <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                        <div class="form-group">
                                            <label runat="server" id="Label12">Corporation Photo <span class="spStar">*</span></label>
                                            &nbsp  &nbsp &nbsp 
                                     <img id="ibtnCorporationphotoadd" src="Images/Add.png" alt="" width="20" class="imgOutLine" />
                                            <br />
                                            <span id="spnFilePath"></span>
                                            <asp:FileUpload ID="fupCorPhoto" runat="server" Style="display: none" onchange="CorPhotoPrev(this);" />
                                            <div class="divImg" id="divImgPre3">
                                                <asp:Image ID="imgCorPhotoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                                <div class="imageOverlay">Click To Upload</div>
                                            </div>

                                        </div>
                                    </div>--%>

                                    <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" style="display: none" id="divCorPhotoNew">
                                        <div class="form-group">
                                            <label runat="server" id="Label5">Corporation Photo 1</label>
                                            &nbsp &nbsp &nbsp
                                    <img id="ibtnCorporationMinus" src="Images/Minus.png" alt="" width="20" class="imgOutLine" />
                                            <br />
                                            <span id="spnFilePhoto1"></span>
                                            <asp:FileUpload ID="fupCorPhoto1" runat="server" Style="display: none" onchange="CorPhotoPrev1(this);" />
                                            <div class="divImg" id="divImgPre4">
                                                <asp:Image ID="imgCorPhoto1Prev" runat="server" alt="Select File" title="Select File" ImageUrl="~/images/FileUpload.png" Width="100%" />
                                                <div class="imageOverlay">Click To Upload</div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="col-sm-3" style="margin-left: -14rem">
                                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server" id="mapview" visible="false">
                                    <div class="form-group">
                                        <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k&callback=Function.prototype" type="text/javascript"></script>
                                        <div id="map">

                                            <script type="text/javascript">
                                                var zipcode = document.getElementById("<%=txtZipcode.ClientID%>").value;
                                                var initialize;
                                                var mapcode = new google.maps.Geocoder();
                                                mapcode.geocode({ 'address': zipcode }, function (results, status) {
                                                    if (status == google.maps.GeocoderStatus.OK) {
                                                        lat = results[0].geometry.location.lat();
                                                        lng = results[0].geometry.location.lng();

                                                    } else {

                                                        document.getElementById('<%= txtAddress1.ClientID%>').value = ' ';
                                                        document.getElementById('<%= txtaddress2.ClientID%>').value = ' ';



                                                    }
                                                    var map = new google.maps.Map(document.getElementById('map'), {
                                                        zoom: 6,
                                                        center: new google.maps.LatLng(lat, lng),
                                                        mapTypeId: google.maps.MapTypeId.ROADMAP
                                                    });

                                                    var marker = new google.maps.Marker({
                                                        position: new google.maps.LatLng(lat, lng),
                                                        map: map,

                                                    });
                                                    google.maps.event.addListener(map, 'click', function (e) {

                                                        var latlng = new google.maps.LatLng(e.latLng.lat(), e.latLng.lng());
                                                        google.maps.event.addListener(map, 'click', function (event) {
                                                            marker.setPosition(event.latLng);
                                                        });
                                                        var geocoder = geocoder = new google.maps.Geocoder();
                                                        geocoder.geocode({ 'latLng': latlng }, function (results, status) {
                                                            if (status == google.maps.GeocoderStatus.OK) {
                                                                if (results[0]) {


                                                                    var control = results[0].formatted_address;
                                                                    const address = control.split(",");
                                                                    var add1 = address[0] + address[1];
                                                                    var add2 = address[2];
                                                                    document.getElementById('<%= txtAddress1.ClientID%>').value = add1;
                                                                    document.getElementById('<%= hfAddres1.ClientID %>').value = add1;
                                                                    document.getElementById('<%= txtaddress2.ClientID%>').value = add2;
                                                                    document.getElementById('<%= hfAddres2.ClientID%>').value = add2;

                                                                }
                                                            }
                                                            for (var component in results[0]['address_components']) {
                                                                for (var i in results[0]['address_components'][component]['types']) {
                                                                    if (results[0]['address_components'][component]['types'][i] == "administrative_area_level_1") {
                                                                        state = results[0]['address_components'][component]['long_name'];
                                                                        document.getElementById('<%= txtState.ClientID%>').value = state;

                                                                    }
                                                                }
                                                            }
                                                            var address = results[0].address_components;
                                                            var zipcode = '';
                                                            for (var i = 0; i < address.length; i++) {
                                                                if (address[i].types.includes("postal_code")) {
                                                                    zipcode = address[i].short_name;
                                                                    document.getElementById('<%= txtZipcode.ClientID%>').value = zipcode;

                                                                }

                                                            }
                                                            fetch('https://api.postalpincode.in/pincode/' + zipcode)
                                                                .then(response => response.json())
                                                                .then(data => {
                                                                    var json = JSON.stringify(data);
                                                                    var obj1 = JSON.parse(json);
                                                                    var arrayLength = obj1.length;
                                                                    for (var i = 0; i < arrayLength; i++) {

                                                                        var Cityblock = obj1[i].PostOffice[i].Block
                                                                        var District = obj1[i].PostOffice[i].District
                                                                        if (Cityblock == "NA") {
                                                                            continue;
                                                                        }
                                                                        if (District == "NA") {
                                                                            continue;
                                                                        }
                                                                        document.getElementById('<%= txtCity.ClientID%>').value = Cityblock;
                                                                        document.getElementById('<%= txtDistrict.ClientID%>').value = District;

                                                                    }

                                                                })

                                                                .catch(error => console.error(error))

                                                        })
                                                    });

                                                });

                                                window.addEventListener('load', initialize);

                                            </script>
                                        </div>

                                    </div>
                                </div>
                                <div class="col-md-12 col-lg-12 col-sm-12 text-right" style="padding-top: 10px; padding-left: 95px; padding-bottom: 20px; margin-left: 15rem">
                                    <div class="form-submit">

                                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="CompanyMaster" TabIndex="20" OnClick="btnSubmit_Click" />
                                        &nbsp 
                                        <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="21" OnClick="btnCancel_Click" Visible="false" />
                                    </div>
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
                        <asp:GridView ID="gvCompanyMaster" runat="server" AllowPaging="True"
                            CssClass="gvv display table table-bordered table-condenced" AutoGenerateColumns="False"
                            DataKeyNames="CorpID,CorpLogo,CorpLogo1,CorpPhoto,CorpPhoto1" PageSize="25000">
                            <Columns>
                                <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <%#Container.DataItemIndex+1 %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="CorpID" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCorpID" runat="server" Text='<%# Bind("CorpID") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Corporate Name" HeaderStyle-CssClass="grdHead" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCorpName" runat="server" Text='<%# Bind("CorpName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Short Name" HeaderStyle-CssClass="grdHead" Visible="true">
                                    <ItemTemplate>
                                        <asp:Label ID="lblShortName" runat="server" Text='<%# Bind("ShortName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Address" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBHAddress" runat="server"
                                            Text='<%#Eval("Address1")+ " " + Eval("Address2")+" " + Eval("City")+ " " + Eval("District")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Address1" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress1" runat="server" Text='<%# Bind("Address1") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Address2" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAddress2" runat="server" Text='<%# Bind("Address2") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Zipcode" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblZipcode" runat="server" Text='<%# Bind("Zipcode") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="City" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCity" runat="server" Text='<%# Bind("City") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="District" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDistrict" runat="server" Text='<%# Bind("District") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="State" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblState" runat="server" Text='<%# Bind("State") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Country" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountry" runat="server" Text='<%# Bind("CountryId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="CountryName" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCountryName" runat="server" Text='<%# Bind("CountryName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Phone Number" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhone" runat="server"
                                            Text='<%#Eval("Phone1")+ " " + Eval("Phone2")%>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Phone1" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhone1" runat="server" Text='<%# Bind("Phone1") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Phone2" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPhone2" runat="server" Text='<%# Bind("Phone2") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Fax" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblFax" runat="server" Text='<%# Bind("Fax") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="MailId" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMailId" runat="server" Text='<%# Bind("MailId") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Application Name" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAppName" runat="server" Text='<%# Bind("AppName") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Corporation Logo" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Image ID="CorpLogo" runat="server" ImageUrl='<%# Eval("CorpLogo") %>' Height="150px" Width="150px" />

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Corporation Logo2" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Image ID="CorpLogo1" runat="server" ImageUrl='<%# Eval("CorpLogo1") %>' Height="150px" Width="150px" />

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Corporation Photo" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Image ID="CorpPhoto" runat="server" ImageUrl='<%# Eval("CorpPhoto") %>' Height="150px" Width="150px" />

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Corporation Photo2" HeaderStyle-CssClass="grdHead" Visible="false">
                                    <ItemTemplate>
                                        <asp:Image ID="CropPhoto1" runat="server" ImageUrl='<%# Eval("CorpPhoto1") %>' Height="150px" Width="150px" />

                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                            runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <asp:HiddenField runat="server" ID="hfCreatedBy" />
            <asp:HiddenField runat="server" ID="hfCorLogoChkVal" Value="0" />
            <asp:HiddenField runat="server" ID="hfCorLogo1ChkVal" Value="0" />
            <asp:HiddenField runat="server" ID="hfCorPhotoChkVal" Value="0" />
            <asp:HiddenField runat="server" ID="hfCorPhoto1ChkVal" Value="0" />

            <asp:HiddenField runat="server" ID="hfCorLoPreImgLk" />
            <asp:HiddenField runat="server" ID="hfCorLoPreImgLk1" />
            <asp:HiddenField runat="server" ID="hfCorPhPreImgLk" />
            <asp:HiddenField runat="server" ID="hfCorPhPreImgLk1" />

            <asp:HiddenField runat="server" ID="hfCorLoRes" />
            <asp:HiddenField runat="server" ID="hfCorLoRes1" />
            <asp:HiddenField runat="server" ID="hfCorPhRes" />
            <asp:HiddenField runat="server" ID="hfCorPhRes1" />

            <asp:HiddenField runat="server" ID="hfCorLoDisp" Value="0" />
            <asp:HiddenField runat="server" ID="hfCorPhDisp" Value="0" />

            <asp:HiddenField runat="server" ID="hfCorLoMinus" Value="0" />
            <asp:HiddenField runat="server" ID="hfCorPhMinus" Value="0" />

            <asp:HiddenField runat="server" ID="hfAddres1" />
            <asp:HiddenField runat="server" ID="hfAddres2" />

    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

