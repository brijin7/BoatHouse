<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ErrorBack.aspx.cs" Inherits="Error_ErrorBack" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Boating - Session Expires</title>
    <meta charset="UTF-8" />
    <meta http-equiv="Expires" content="0" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Pragma" content="no-cache" />

    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/meyer-reset/2.0/reset.min.css" />
    <link rel="stylesheet" href="ERRORstyle.css" />

     <script type="text/javascript">
         function NoBack() {
             window.history.forward(0);
             window.location.href = "ErrorPage.aspx";
         }

         setTimeout("NoBack()", 0);

     </script>
     <script type="text/javascript">
         function HomePage() {
             window.location.href = Session["LogOutUrl"].ToString();
         }

     </script>
</head>
<body onload="NoBack();" onunload="NoBack();">
    <form runat="server">
        <!-- partial:index.partial.html -->
        <div class="not-found parallax">
            <div class="sky-bg">
                <img src="../images/TTDCLogo.svg" alt="Island" style="padding-left: 300px;height: 115px;" />
            </div>

            <a class="wave-island" href="#">
                <img src="island.svg" alt="Island" onclick="HomePage()" />
            </a>

            <div class="wave-5"></div>
            <div class="wave-lost wrp">
                <span>4</span>
                <span>0</span>
                <span>4</span>
            </div>
            <div class="wave-4"></div>
            <div class="wave-boat">
                <img class="boat" src="Errorboat.svg" alt="Boat" style="width: 300px" />
            </div>
            <div class="wave-1"></div>
            <div class="wave-message">
                <p>Your're lost</p>
                <asp:LinkButton runat="server" Text="Click to return" ID="lnkHomePage" OnClick="lnkHomePage_Click"
                    Style="color: #cbe29f"></asp:LinkButton>

            </div>
        </div>
               <%-- Newly implemented for CSRF Validation--%>
        <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
        <%-- Newly implemented for CSRF Validation--%>


    </form>
</body>
</html>
