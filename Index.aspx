<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css" integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Roboto:300,400,500,700" />
    <%-- <link rel="stylesheet" type="text/css" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/3.7.0/animate.css" />--%>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.19/css/dataTables.bootstrap.min.css" />
    <link rel="stylesheet" type="text/css" href="https://use.fontawesome.com/releases/v5.7.2/css/all.css" integrity="sha384-fnmOCqbTlWIlj8LyTjo7mOUStjsKC4pOpQbqyi7RrhN7udi9RwhKkMHpvLbHG9Sr" crossorigin="anonymous" />
    <script type="text/javascript" src="Scripts/sweetalert.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="col-sm-12" style="font-weight: bolder; font-size: 24px; text-align: center; margin-top: 5%;">
            <img class="img-fluid" src="images/paypre logo.png" alt="Boating" style="height: 80px !important" />
        </div>
        <div style="margin-top: 3%;">
            <div class="container-fluid">
                <div class="cover-flex">
                    <div class="box-div">
                        <div class="col-md-12 col-xs-12">
                            <div class="form animated slideInLeft" id="divLogin" runat="server">
                                <div class="col-sm-2 offset-sm-5 form-group" style="border: 1px solid gray; border-radius: 8%;">
                                    <div class="form-div text-center">
                                        <asp:Label ID="lblSignIn" runat="server" CssClass="ui icon header title" Style="font-weight: bolder; font-size: 24px;"> Sign In</asp:Label>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblUsername" runat="server" CssClass="lbl">Mobile No / Email</asp:Label>
                                        <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" AutoComplete="Off" MaxLength="30" TabIndex="1"
                                            AutoPostBack="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtUserName"
                                            ValidationGroup="Login" SetFocusOnError="True" CssClass="vError">Enter Mobile No / Email</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <asp:Label ID="lblPassword" runat="server" CssClass="lbl">Password</asp:Label>
                                        <div class="input-group">
                                            <asp:TextBox runat="server" CssClass="form-control passBox1" ID="txtPassword"
                                                AutoComplete="Off" TabIndex="2" TextMode="Password" MaxLength="20"></asp:TextBox>

                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtPassword"
                                            ValidationGroup="Login" SetFocusOnError="True" CssClass="vError">Enter Password</asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group" style="text-align: center !important">
                                        <asp:Button ID="btnSubmit" runat="server" Text="Sign In" ValidationGroup="Login"
                                            class="btn btn-primary" TabIndex="3" OnClick="btnSubmit_Click" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-sm-12" style="font-weight: bolder; font-size: 24px; text-align: center; margin-top: 5%;">
            BOATING - TEST SERVER
        </div>
        <%-- Newly implemented for CSRF Validation--%>
        <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
        <%-- Newly implemented for CSRF Validation--%>
    </form>
</body>
</html>
