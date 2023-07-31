<%@ Page Title="" Language="C#" MasterPageFile="~/Boating/Boating.master" AutoEventWireup="true" CodeFile="PrinterSetup.aspx.cs" Inherits="Boating_PrinterSetup" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <%--    <div class="form-body">

        <div runat="server" id="divEntry" style="width: 100%">
            <div class="row">
                <div class="col-md-4 col-sm-4">
                    <h3 id="Time" style="display: inline; float: left; color: black;"></h3>
                </div>
                <div class="col-md-4 col-sm-4 text-center">
                    <h5 class="pghr" style="text-align: center; font-size: 25px; display: inline;">Printer Setup </h5>
                </div>
            </div>

            <hr />

            <asp:Label ID="lblPath" runat="server"></asp:Label>
        </div>
    </div>--%>
    <%-- Newly implemented for CSRF Validation--%>
    <input type="hidden" name="__RequestVerificationToken" value="<%= GetAntiForgeryToken() %>" />
    <%-- Newly implemented for CSRF Validation--%>
</asp:Content>

