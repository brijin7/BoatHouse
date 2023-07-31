<%@ Page Title="" Language="C#" Async="true" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="~/Common/SadminUser.aspx.cs" Inherits="SadminUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">



    <style type="text/css">
        .rbl input[type="radio"] {
            margin-left: 10px;
            margin-right: 6px;
        }

        .imageOverlay {
            background: rgba(245, 245, 245, 0.4);
            height: 20px;
            position: absolute;
            bottom: 0px;
            width: 100%;
            text-align: center;
            color: black;
            font-size: 12px;
            font-weight: 500;
        }

        .divImg {
            /*width: 150px;*/
            position: relative;
        }

            .divImg:hover {
                box-shadow: 0 0 20px #dddddd;
                cursor: pointer;
            }
    </style>
    <script type="text/javascript">
        function ShowImagePreview(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#<%=imgEmpPhotoPrev.ClientID%>').prop('src', e.target.result);
                            $('#<%=hfImageCheckValue.ClientID%>').val("1");
                };
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            let showPass = document.querySelector("#show_password");
            if (showPass) {
                showPass.addEventListener("click", function () {

                    let passBox = document.querySelector(".passBox1");

                    if (passBox.type === "password") {
                        passBox.type = "text";
                        $('.icon').removeClass('fa fa-eye-slash').addClass('fa fa-eye');
                    }
                    else {
                        passBox.type = "password";
                        $('.icon').removeClass('fa fa-eye').addClass('fa fa-eye-slash');
                    }
                })
            }
        });
        $(function () {
            var fileupload = $('#<%=fupEmpLink.ClientID%>');
                    var image = $('#divImgPreview');
                    image.click(function () {
                        fileupload.click();
                    });
                });
    </script>

    <div class="form-body">
        <h5 class="pghr">Sadmin User <span style="float: right;">
            <asp:LinkButton ID="lbtnNew" CssClass="lbtnNew" runat="server" OnClick="lbtnNew_Click"> 
                        <i class="fas fa-plus-circle"></i> Add New</asp:LinkButton></span> </h5>
        <hr />
        <div id="divEntry" runat="server" visible="false">
            <div class="row p-2">
                <div class="col-sm-9 col-md-9 col-lg-9 col-xs-12">
                    <span class="frmhdr">Personal Details</span>
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12" style="display: none">
                                <div class="form-group">
                                    <label for="txtConfigurationType">EmpId <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtEmpId" runat="server" CssClass="form-control">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="txtConfigurationType">First Name <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtEmployeeFirstName" runat="server" CssClass="form-control" TabIndex="1" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmployeeFirstName" ForeColor="Red"
                                        ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEmployeeFirstName"
                                        ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter First Name</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="txtEmployeeLastName">Last Name <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtEmployeeLastName" runat="server" CssClass="form-control" TabIndex="2" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtEmployeeLastName" ForeColor="Red"
                                        ValidationExpression="[a-zA-Z0-9 ]*$" ErrorMessage="Special Characters Not Allowed" CssClass="vError" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtEmployeeLastName"
                                        ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Last Name</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="txtMobNo">Mobile No <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtMobNo" runat="server" CssClass="form-control" TabIndex="3" MaxLength="10" AutoComplete="Off" onkeypress="return isNumber(event);">    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtMobNo"
                                        ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Mobile No</asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                        ControlToValidate="txtMobNo" ErrorMessage="Invalid"
                                        ValidationExpression="[0-9]{10}" ForeColor="Red"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                            <div class="col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="txtemailid">Email Id</label>
                                    <asp:TextBox ID="txtemailid" runat="server" CssClass="form-control" TabIndex="4" MaxLength="40" AutoComplete="Off">
                                    </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtemailid"
                                        SetFocusOnError="True" CssClass="vError" ValidationExpression="^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"
                                        Display="Dynamic" ErrorMessage="Invalid email address" ValidationGroup="Employee" />
                                </div>
                            </div>
                        </div>
                        <div class="row p-2">
                            <div class="col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="txtAadharid">Aadhar Id</label>
                                    <asp:TextBox ID="txtAadharid" runat="server" CssClass="form-control" TabIndex="5" MaxLength="12" AutoComplete="Off" onkeypress="return isNumber(event);">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 col-lg-3 col-sm-12">
                            </div>
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="EmpUserName">User Name <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" TabIndex="6" MaxLength="50" AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txtUserName"
                                        ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter User Name</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="EmpPassword">Password <span class="spStar">*</span></label>
                                    <div class="input-group-addon" style="display: flex;">
                                        <asp:TextBox ID="txtEmpPassword" runat="server" CssClass="form-control passBox1" TabIndex="7" MaxLength="50" TextMode="Password"
                                            AutoComplete="Off" onkeypress="return LettersWithSpaceOnly(event);">
                                        </asp:TextBox>
                                        <div class="input-group-prepend">
                                            <div class="input-group-text p-0">
                                                <button id="show_password" class="btn btn-light pt-1" type="button" style="height: 28px; width: 40px;">
                                                    <span class="fa fa-eye-slash icon p-0"></span>
                                                </button>
                                            </div>
                                        </div>
                                    </div>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtEmpPassword"
                                        ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Enter Password</asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>
                    <span class="frmhdr">Department Details</span>
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="ddlEmployeeId">Employee Type <span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlEmpType" runat="server" CssClass="form-control" TabIndex="8">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="ddlEmpType"
                                        ValidationGroup="Employee" SetFocusOnError="True" InitialValue="Select Employee Type" CssClass="vError">Select Employee Type</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="ddlshift">Shift <span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlshift" runat="server" CssClass="form-control" TabIndex="9">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="ddlshift"
                                        ValidationGroup="Employee" SetFocusOnError="True" InitialValue="Select Shift" CssClass="vError">Select Shift</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="ddlempdesignation">Employee Designation <span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlEmpDesignation" runat="server" CssClass="form-control" TabIndex="11">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="ddlEmpDesignation"
                                        ValidationGroup="Employee" SetFocusOnError="True" InitialValue="Select Designation" CssClass="vError">Select Designation</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="txtDOJ">DOJ <span class="spStar">*</span></label>
                                    <asp:TextBox ID="txtDOJ" runat="server" CssClass="form-control myCal" TabIndex="12" MaxLength="50" AutoComplete="Off">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtDOJ"
                                        ValidationGroup="Employee" SetFocusOnError="True" CssClass="vError">Select DOJ</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-xl-3 col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="ddlrole">Role <span class="spStar">*</span></label>
                                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" TabIndex="13">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddlRole"
                                        ValidationGroup="Employee" SetFocusOnError="True" InitialValue="Select Role" CssClass="vError">Select Role</asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                                <div class="form-group">
                                    <label>Mobile App Access</label>
                                    <asp:RadioButtonList ID="rblMobileAccess" runat="server" RepeatDirection="Horizontal" TabIndex="14" CssClass="rbl">
                                        <asp:ListItem Value="Y">Yes</asp:ListItem>
                                        <asp:ListItem Value="N" Selected="true">No</asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                            </div>
                        </div>
                    </div>
                    <span class="frmhdr">Address Details</span>
                    <div class="mydivbrdr">
                        <div class="row p-2">
                            <div class="col-md-3 col-lg-3 col-sm-12">
                                <div class="form-group">
                                    <label for="lblZipCode">ZIP Code</label>
                                    <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" TabIndex="15" MaxLength="6" AutoComplete="auto"
                                        onkeypress="return isNumber(event);" OnTextChanged="txtZipCode_TextChanged" AutoPostBack="true">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 col-lg-3 col-sm-12" id="divCity" runat="server" visible="false">
                                <div class="form-group">
                                    <label for="lblCity">City</label>
                                    <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" TabIndex="16" MaxLength="50" AutoComplete="auto">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 col-lg-3 col-sm-12" id="divDistrict" runat="server" visible="false">
                                <div class="form-group">
                                    <label for="lblDistrict">District</label>
                                    <asp:TextBox ID="txtDistrict" runat="server" CssClass="form-control" TabIndex="17" MaxLength="12" AutoComplete="auto">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3 col-lg-3 col-sm-12" id="divState" runat="server" visible="false">
                                <div class="form-group">
                                    <label for="lblState">State</label>
                                    <asp:TextBox ID="txtState" runat="server" CssClass="form-control" TabIndex="18" MaxLength="50" AutoComplete="auto">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6 col-lg-6 col-sm-12" id="divAddressDet" runat="server" visible="false">
                                <div class="form-group">
                                    <label runat="server" id="Label1">Door/Flat No, Street Name</label>
                                    <asp:TextBox ID="txtAddr1" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto"
                                        MaxLength="13" TextMode="MultiLine" Font-Size="14px" TabIndex="19" Rows="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6 col-lg-6 col-sm-12" id="divAddressDet1" runat="server" visible="false">
                                <div class="form-group">
                                    <label runat="server" id="Label2">Location/Area Name</label>
                                    <asp:TextBox ID="txtAddr2" runat="server" CssClass="form-control inputboxstyle" AutoComplete="auto" Font-Size="Small"
                                        MaxLength="10" TextMode="MultiLine" TabIndex="20" Rows="1">
                                    </asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3 col-xs-12">
                    <div class="col-sm-10 offset-sm-1 p-2">
                        <div class="panel panel-success">
                            <div class="panel-heading">Employee Photo <span class="spStar">*</span></div>
                            <div class="panel-body">
                                <asp:FileUpload ID="fupEmpLink" runat="server" TabIndex="21" Style="display: none" onchange="ShowImagePreview(this);" />
                                <div class="divImg" id="divImgPreview">
                                    <asp:Image ID="imgEmpPhotoPrev" runat="server" alt="Select File" title="Select File" ImageUrl="~/Images/EmptyImage.png" Width="100%" />
                                    <div class="imageOverlay">Click To Upload</div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12 p-2" runat="server" id="divMapView" visible="false">
                        <div class="form-group">
                            <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyB56Km4bH3DEKxXLRZBltsTIm3eYgPqt0k&callback=Function.prototype" type="text/javascript"></script>
                            <div id="map">
                                <script type="text/javascript">
                                    var zipcode = document.getElementById("<%=txtZipCode.ClientID%>").value;
                                    var initialize;
                                    var mapcode = new google.maps.Geocoder();
                                    mapcode.geocode({ 'address': zipcode }, function (results, status) {
                                        if (status == google.maps.GeocoderStatus.OK) {
                                            lat = results[0].geometry.location.lat();
                                            lng = results[0].geometry.location.lng();
                                        }
                                        else {
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
                                                                document.getElementById('<%= txtAddr1.ClientID%>').value = add1;
                                                                document.getElementById('<%= txtAddr2.ClientID%>').value = add2;
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
                                    window.addEventListener('load', initialize);
                                </script>
                            </div>
                        </div>
                    </div>
                    <div class="col-sm-12" style="position: relative; bottom: 0px; right: 0px;">
                        <div class="form-submit">
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" class="btn btn-primary" ValidationGroup="Employee" TabIndex="22" OnClick="btnSubmit_Click" />
                            <asp:Button ID="btnCancel" runat="server" Text="Cancel" CausesValidation="false"
                                class="btn  btn-danger" TabIndex="23" OnClick="btnCancel_Click" />
                        </div>
                    </div>
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
                <asp:GridView ID="gvmstrEmployee" runat="server" AutoGenerateColumns="False"
                    CssClass="gvv display table table-bordered table-condenced"
                    DataKeyNames="UserId,EmpId,EmpType,EmpTypeName,EmpFirstName,EmpLastName,EmpName,
                            EmpDesignation,BranchId,DesignationName,EmpMobileNo,Address1,Address2,Zipcode,City,District,
                            State,DOJ,EmpAadharId,EmpMailId,
                            EmpPhotoLink,ShiftId,ShiftName,RoleId,RoleName,UserName,Password,MobApp,ActiveStatus,Createdby">
                    <Columns>
                        <asp:TemplateField HeaderText="Sl No." HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Id" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserId" runat="server" Text='<%# Bind("UserId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="User Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%# Bind("UserName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Employee Name" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpName" runat="server" Text='<%# Bind("EmpName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Designation" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblDesignationName" runat="server" Text='<%# Bind("DesignationName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Mobile No.">
                            <ItemTemplate>
                                <asp:Label ID="lblempMobileNo" runat="server" Text='<%# Bind("EmpMobileNo") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Aadhar Id" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblEmpAadharId" runat="server" Text='<%# Bind("EmpAadharId") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Role" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Edit" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:ImageButton ID="ImgBtnEdit" ForeColor="#512c88" CausesValidation="false" Font-Underline="false" Width="20" CssClass="imgOutLine"
                                    runat="server" Font-Bold="true" ImageUrl="~/images/Edit.png" OnClick="ImgBtnEdit_Click"
                                    EnableViewState="false" Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="5px" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Active (or) Inactive" HeaderStyle-CssClass="grdHead">
                            <ItemTemplate>
                                <asp:LinkButton ID="ImgBtnDelete" ForeColor="#198606" CausesValidation="false" Font-Underline="false" Text="Active"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnDelete_Click"
                                    OnClientClick="return confirm('Are you sure to Inactive this record?');"
                                    Visible='<%# Eval("ActiveStatus").ToString() == "A"? true: false %>' />
                                <asp:LinkButton ID="ImgBtnUndo" ForeColor="#e41515" CausesValidation="false" Font-Underline="false" Text="Inactive"
                                    runat="server" Font-Bold="true" OnClick="ImgBtnUndo_Click"
                                    OnClientClick="return confirm('Are you sure to Active this record?');" Visible='<%# Eval("ActiveStatus").ToString() == "D"? true: false %>' />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <asp:HiddenField runat="server" ID="hfResponse" />
    <asp:HiddenField runat="server" ID="hfPrevImageLink" />
    <asp:HiddenField runat="server" ID="hfImageCheckValue" Value="0" />
    <asp:HiddenField runat="server" ID="hfPassword" />


    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

