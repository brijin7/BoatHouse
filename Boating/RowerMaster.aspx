<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="RowerMaster.aspx.cs" Inherits="Boating_RowerMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <style>
        #map {
            height: 250px;
            width: 250px;
            border: groove;
            color: black;
            position: sticky;
        }
    </style>
    <script type="text/javascript">
        function ShowImagePreview(input) {
            var fup = document.getElementById("<%= fupEmpLink.ClientID %>");
            var fileName = fup.value;
            var ext = fileName.substring(fileName.lastIndexOf('.') + 1);
            if (ext == "gif" || ext == "GIF" || ext == "PNG" || ext == "png" || ext == "jpg" || ext == "JPG" || ext == "bmp" || ext == "BMP" || ext == "jpeg" || ext == "JPEG") {
                if (input.files && input.files[0]) {
                    var reader = new FileReader();
                    reader.onload = function (e) {
                        $('#<%=imgEmpPhotoPrev.ClientID%>').prop('src', e.target.result);
                        $('#<%=hfImageCheckValue.ClientID%>').val("1");
                    };
                    reader.readAsDataURL(input.files[0]);
                    return true;
                }
            }
            else {
                swal("Please, Upload Gif, Jpg, Jpeg or Bmp Images only");
                fup.focus();
                return false;
            }
        }
        $(function () {
            var fileupload = $('#<%=fupEmpLink.ClientID%>');
            var image = $('#divImgPreview');
            image.click(function () {
                fileupload.click();
            });
        });
    </script>
    <div class="form-body">
        <h5 class="pghr">Rower/Driver Master <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton>
            <asp:LinkButton ID="lbtnGenRowerCard" CssClass="lbtnNew" runat="server" OnClick="lbtnGenRowerCard_Click"> 
                <i class="fas fa-receipt"></i> Generate Rower ID Card</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="row p-2">
                <div class="col-sm-9 col-md-9 col-lg-9 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading">Rower Details</div>
                        <div class="panel-body">
                            <div class="row">
                                <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12" runat="server" visible="false">
                                    <div class="form-group">
                                        <label for="txtConfigurationType">Rower id <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtRowerId" runat="server" CssClass="form-control" TabIndex="1" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                        </asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtConfigurationType">Rower Name <span class="spStar">*</span></label>
                                        <asp:TextBox ID="txtRower" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                        </asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtRower"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Rower Name</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtMobNo">Mobile No</label>
                                        <asp:TextBox ID="txtMobNo" runat="server" CssClass="form-control" TabIndex="3" MaxLength="10" AutoComplete="Off" onkeypress="return isNumber(event);">    </asp:TextBox>
                                        <%-- 
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMobNo"
                                            ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Mobile No</asp:RequiredFieldValidator>
                                          <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"  ControlToValidate="txtMobNo"
                                  ValidationExpression="^[2-9]{2}[0-9]{8}$"  ValidationGroup="PayUPI"
                                SetFocusOnError="True" CssClass="vError"  ErrorMessage="Please Enter Correct Number"></asp:RegularExpressionValidator>--%>
                                    </div>
                                </div>
                                <div class="col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtemailid">Email Id</label>
                                        <asp:TextBox ID="txtemailid" runat="server" CssClass="form-control" TabIndex="4" MaxLength="40" AutoComplete="Off"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtemailid"
                                            SetFocusOnError="True" CssClass="vError" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                            Display="Dynamic" ErrorMessage="Invalid email address" ValidationGroup="Employee" />
                                    </div>
                                </div>
                                <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="lblRowerCategory">Rower / Driver <span class="spStar">*</span></label>
                                        <asp:DropDownList ID="ddlDriverType" runat="server" CssClass="form-control" TabIndex="5">
                                            <asp:ListItem Value="0">Select Rower Category</asp:ListItem>
                                            <asp:ListItem Value="1">Rower</asp:ListItem>
                                            <asp:ListItem Value="2">Driver</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlDriverType"
                                            ValidationGroup="Employee" SetFocusOnError="True" InitialValue="0" CssClass="vError">Select Rower category</asp:RequiredFieldValidator>
                                    </div>
                                </div>

                            </div>

                            <div class="row">
                                <div class="col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="txtAadharid">Aadhar Id</label>
                                        <asp:TextBox ID="txtAadharid" runat="server" CssClass="form-control" TabIndex="5" MaxLength="12" AutoComplete="Off" onkeypress="return isNumber(event);"></asp:TextBox>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="txtAadharid" runat="server" Font-Size="Small"
                                            ErrorMessage="Only Numbers allowed and Exactly 12 Digit Number" ValidationExpression="^[0-9]{12}$" ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="ddlRowerId">Rower Type <span class="spStar">*</span></label>
                                        <asp:DropDownList ID="ddlRowerType" runat="server" CssClass="form-control" TabIndex="6">
                                        </asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlRowerType"
                                            ValidationGroup="Employee" SetFocusOnError="True" InitialValue="Select Rower Type" CssClass="vError">Select Rower Type</asp:RequiredFieldValidator>
                                    </div>
                                </div>
                                <div class="col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="lblZipCode">ZIP Code</label>
                                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" TabIndex="7" MaxLength="6" AutoComplete="Off"
                                            onkeypress="return isNumber(event);" OnTextChanged="txtZipCode_TextChanged" AutoPostBack="true">
                                        </asp:TextBox>
                                        <%--<asp:RegularExpressionValidator ID="RegularExpressionValidator6" ControlToValidate="txtZipCode" runat="server" Font-Size="Small"
                                            ErrorMessage="Only Numbers allowed and Exactly 6 Digit Number" ValidationExpression="^[0-9]{6}$" ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError"></asp:RegularExpressionValidator>--%>
                                    </div>
                                </div>
                            </div>

                            <div class="row" id="divAddrDet" runat="server" visible="false">
                                <div class="col-md-6 col-lg-6 col-sm-12">
                                    <div class="form-group">
                                        <label runat="server" id="Label1">Door/Flat No, Street Name</label>
                                        <asp:TextBox ID="txtAddr1" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto" MaxLength="10" TextMode="MultiLine" Font-Size="14px" TabIndex="8" Rows="1"></asp:TextBox>
                                        <asp:RegularExpressionValidator runat="server" ID="valInput" Font-Size="Small"
                                            ControlToValidate="txtAddr1"
                                            ValidationExpression="^[\s\S]{0,300}$"
                                            ErrorMessage="Please enter a maximum of 300 characters"
                                            Display="Dynamic" ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-md-6 col-lg-6 col-sm-12">
                                </div>
                                <div class="col-md-6 col-lg-6 col-sm-12">
                                    <div class="form-group">
                                        <label runat="server" id="Label2">Location/Area Name</label>
                                        <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto" Font-Size="Small" MaxLength="10" TextMode="MultiLine" TabIndex="9" Rows="1">
                                        </asp:TextBox>
                                        <asp:RegularExpressionValidator runat="server" ID="RegularExpressionValidator1"
                                            ControlToValidate="txtAddr2"
                                            ValidationExpression="^[\s\S]{0,300}$"
                                            ErrorMessage="Please enter a maximum of 300 characters"
                                            Display="Dynamic" ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError"></asp:RegularExpressionValidator>
                                    </div>
                                </div>
                                <div class="col-md-6 col-lg-6 col-sm-12">
                                </div>
                                <div class="col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="lblCity">City</label>
                                        <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" TabIndex="10" MaxLength="50" AutoComplete="Off"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="lblDistrict">District</label>
                                        <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control" TabIndex="11" MaxLength="12" AutoComplete="Off"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3 col-lg-3 col-sm-12">
                                    <div class="form-group">
                                        <label for="lblState">State</label>
                                        <asp:TextBox ID="txtState" runat="server" CssClass="form-control" TabIndex="12" MaxLength="50" AutoComplete="Off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-sm-2 col-md-2 col-lg-2 col-xs-12">
                    <div class="panel panel-success">
                        <div class="panel-heading"><i class="fa fa-ship" aria-hidden="true"></i>Rower Image</div>
                        <div class="panel-body">
                            <div class="col-sm-12 p-0">
                                <asp:FileUpload ID="fupEmpLink" runat="server" Style="display: none" onchange="ShowImagePreview(this);" />
                                <div class="divImg" id="divImgPreview">
                                    <asp:Image ID="imgEmpPhotoPrev" runat="server" alt="Select File" title="Select File" TabIndex="13" ImageUrl="../images/EmptyImage.png" Width="100%" />
                                    <div class="imageOverlay">Click To Upload</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div id="divMapView" runat="server" visible="false">
                        <div class="col-sm-12 p-0" style="margin-left: -1rem;">
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12" runat="server">
                                <div class="form-group">
                                    <script src="https://maps.google.com/maps/api/js?sensor=false&key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k" type="text/javascript"></script>
                                    <div id="map" tabindex="11">

                                        <script type="text/javascript">
                                            var zipcode = document.getElementById("<%=txtZipCode.ClientID%>").value;
                                            var mapcode = new google.maps.Geocoder();
                                            mapcode.geocode({ 'address': zipcode }, function (results, status) {
                                                if (status == google.maps.GeocoderStatus.OK) {
                                                    lat = results[0].geometry.location.lat();
                                                    lng = results[0].geometry.location.lng();

                                                } else {

                                                    document.getElementById('<%= txtAddr1.ClientID%>').value = ' ';
                                                    document.getElementById('<%= txtAddr2.ClientID%>').value = ' ';
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
                                                                document.getElementById('<%=txtAddr1.ClientID%>').value = add1;
                                                                document.getElementById('<%= hfAddres1.ClientID %>').value = add1;
                                                                document.getElementById('<%= txtAddr2.ClientID%>').value = add2;
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
                                                                document.getElementById('<%= txtZipCode.ClientID%>').value = zipcode;

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
                                            google.maps.event.addDomListener(window, 'load', initialize);

                                        </script>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-md-12 col-lg-12 col-sm-12 text-right">
                <div class="form-submit">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Employee" TabIndex="14" OnClick="btnSubmit_Click" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false" class="btn  btn-danger" TabIndex="15" OnClick="btnCancel_Click" />

                </div>
            </div>
        </div>

        <div runat="server" id="divGrid1">
            <div style="margin-left: auto; margin-right: auto; text-align: center;">
                <asp:Label ID="lblGridMsgg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
            </div>
        </div>
        <div id="divGrid" runat="server">
            <div class="table-responsive">
                <div style="margin-left: auto; margin-right: auto; text-align: center;">
                    <asp:Label ID="lblGridMsg" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>
                </div>
                <div id="divSearch" runat="server" style="text-align: right;">
                    Search :
                        <asp:TextBox ID="txtSearch" runat="server" Font-Size="20px" OnTextChanged="txtSearch_TextChanged" AutoComplete="off" AutoPostBack="true"></asp:TextBox>
                </div>
                <asp:GridView ID="gvRowerMstr" runat="server" AutoGenerateColumns="False" DataKeyNames="RowerId,RowerType,DriverCategory" AllowPaging="True"
                    CssClass="CustomGrid table table-bordered table-condenced" PageSize="25000" OnRowDataBound="gvRowerMstr_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%-- <%# Container.DataItemIndex + 1 %>--%>
                                <asp:Label ID="lblRowNumber" runat="server" Text='<%# Bind("RowNumber") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rower Id" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerId" runat="server" Text='<%# Bind("RowerId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rower/Driver Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerName" runat="server" Text='<%# Bind("RowerName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Rower / Driver" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblCategory" runat="server" Text='<%# Bind("DriverCategory") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mobile No.">
                            <ItemTemplate>
                                <asp:Label ID="lblMobileNo" runat="server" Text='<%# Bind("MobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Email" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblMailId" runat="server" Text='<%# Bind("MailId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Aadhar Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAadharId" runat="server" Text='<%# Bind("AadharId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower Type" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerType" runat="server" Text='<%# Bind("RowerType") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rower Type ">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerTypeName" runat="server" Text='<%# Bind("RowerTypeName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Address" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRowerAddress" runat="server" Text='<%#Eval("Address1")+ " " + Eval("Address2")+" " + Eval("City")+ " " + Eval("District")+" "+Eval("State")+" "+ Eval("ZipCode")%>'></asp:Label>
                            </ItemTemplate>
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

                        <asp:TemplateField HeaderText="Zip Code" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblZipCode" runat="server" Text='<%# Bind("ZipCode") %>'></asp:Label>
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

                        <asp:TemplateField HeaderText="PhotoLink" HeaderStyle-CssClass="grdHead" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblPhotoLink" runat="server" Text='<%# Bind("PhotoLink") %>'></asp:Label>
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
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click1"
                                    EnableViewState="false" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
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
                    <HeaderStyle CssClass="gvHead" />
                    <AlternatingRowStyle CssClass="gvRow" />
                    <PagerStyle HorizontalAlign="Center" CssClass="gvPage" />
                    <FooterStyle BackColor="White" ForeColor="#000066" Font-Bold="true" HorizontalAlign="Right" />
                </asp:GridView>
                <div>
                    <asp:Button ID="back" runat="server" CssClass="btn btn-color" Visible="true" Text="← Previous" OnClick="back_Click" Enabled="false" />
                    &nbsp
                        <asp:Button ID="Next" Visible="true" CssClass="btn btn-color" runat="server" OnClick="Next_Click" Text="Next →" />

                </div>
                <div>
                    <asp:Button ID="searchback" runat="server" CssClass="btn btn-color" Visible="false" OnClick="searchback_Click" Text="← Previous" Enabled="false" />
                    &nbsp
                        <asp:Button ID="searchNext" CssClass="btn btn-color" runat="server" Visible="false" OnClick="searchNext_Click" Text="Next →" />
                    &nbsp
                         <asp:Button ID="BackToList" Visible="false" CssClass="btn btn-color" runat="server" OnClick="BackToList_Click" Text="← Back To List" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hfCreatedBy" runat="server" />
    <asp:HiddenField ID="hfBoatHouseId" runat="server" />
    <asp:HiddenField ID="hfBoatHouseName" runat="server" />
    <asp:HiddenField ID="hfstartvalue" runat="server" />
    <asp:HiddenField ID="hfendvalue" runat="server" />
    <asp:HiddenField ID="hfSearchValue" runat="server" />
    <asp:HiddenField ID="hfSearchEndValue" runat="server" />
    <asp:HiddenField runat="server" ID="hfResponse" />
    <asp:HiddenField runat="server" ID="hfPrevImageLink" />
    <asp:HiddenField runat="server" ID="hfImageCheckValue" Value="0" />
    <asp:HiddenField runat="server" ID="hfAddres1" />
    <asp:HiddenField runat="server" ID="hfAddres2" />
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
</asp:Content>

