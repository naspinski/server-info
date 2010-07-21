<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<string>>" %>
<%@ Import Namespace="ServerInfo.WebUI.HtmlHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="content">
        <%= Html.Partial("QueryInput") %>
        <fieldset class="messages">
            <% foreach (string s in Model) { %> <%= s%> <%} %>
        </fieldset>
    </div>
    <div id="sidebar">
        <fieldset class="box">
            <legend>info</legend>
            <%= Html.WMIInfo() %>
        </fieldset>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadTitle" runat="server">
    Manual WMI Query
</asp:Content>
