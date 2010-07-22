<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ServerInfo.WebUI.Models.DisplayAndServers>" %>
<%@Import Namespace="System.Reflection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <fieldset class="clear">
            <legend><i class="arrow_blue"></i>recent cached data [<%= Model.Timestamp.ToString() %>]</legend>
            <table>
                <tr>
                    <% foreach (string pName in Model.Display.ActivePropertyNames)
                       { %> <th>
                            <% if (Model.Sorters.Contains(pName))
                               { %>
                               <%= Html.ActionLink(pName, "Index", new { SortBy = pName, 
                                    SortDir = (Model.SortDir.ToLower().Equals("up") ? "Down" : "Up") })%>
                            <% } else { %> 
                                <%= pName %>
                            <% } %>
                            </th> 
                    <% } %>
                    <th></th>
                </tr>
                <% foreach (ServerSummary server in Model.Servers)
                   { %>
                    <tr>
                        <% foreach (string pName in Model.Display.ActivePropertyNames) 
                           { %>
                               <td><%= server.Strings[pName] %></td>
                        <% } %>
                        <td class="no_border">
                            <%= Html.ActionLink(" ", "Delete", 
                                new { Ip = server.Strings["Ip"], }, 
                                new { @class = "delete", title = "delete" }) %>
                        </td>
                    </tr>
                <% } %>
            </table>
        </fieldset>
    </div>
    <div id="sidebar">
        <fieldset class="box">
            <legend>actions</legend>
            <ul class="list_vertical">
                <li><%= Html.ActionLink("Refresh", "Refresh") %></li>
                <li><%= Html.ActionLink("New IP", "New", "Action", null, new { id="aNew" })  %></li>
            </ul>
        </fieldset>
    </div>

    <div id="modal" class="jqmWindow">loading...</div>

    <script type="text/javascript">
        $().ready(function () {
            $('#modal').jqm({ ajax: '/Action/New', trigger: $('#aNew') });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadTitle" runat="server">
    ServerInfo
</asp:Content>
