<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ServerInfo.WebUI.Models.DisplayAndServers>" %>
<%@ Import Namespace="ServerInfo.WebUI.HtmlHelpers" %>
<%@Import Namespace="System.Reflection" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="content">
        <fieldset class="clear">
            <% if(Model.Servers.Count() == 0) { %>
                <legend><br />Please add a <a href="#" class="newIp">New Ip Address</a></legend>
            <% } else { %>
            <legend>Last Updated [<%= Model.Timestamp.ToString() %>]</legend>
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
            <% } %>
        </fieldset>
    </div>
    <div id="sidebar">
        <%= Html.Navigation(Request.RequestContext.RouteData.Values) %>
        <fieldset class="box">
            <legend>actions</legend>
            <ul class="list_vertical">
                <li><%= Html.ActionLink("refresh", "Refresh") %></li>
                <li><%= Html.ActionLink("new ip", "New", "Action", null, new { id="aNew", @class="newIp" })  %></li>
                <li><%= Html.ActionLink("multiple new ips", "BatchNew", "Action", null, new { id="aNews", @class="newIps" })  %></li>
            </ul>
        </fieldset>
    </div>

    <div id="test"></div>
    
    <div id="modal" class="jqmWindow">loading...</div>
    <div id="modalIps" class="jqmWindow">loading...</div>
    
    <div id="newOwner" class="jqmWindow">
        <div>
            <div>
                <%= Html.Hidden("NewOwnerId") %>
                <label for="NewOwners">Add Owners</label>
                <%= Html.TextArea("NewOwners") %>
            </div>
            <div class="align_center">
                <input type="button" value="submit" id="submitNewOwners" />
                <input type="button" value="cancel" class="jqmClose" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $().ready(function () {
            $('#modal').jqm({
                ajax: '<%= Url.Action("New", "Action") %>',
                trigger: $('.newIp'),
                onShow: function (h) { h.w.slideDown(); },
                onHide: function (h) { h.w.slideUp('medium', function () { if (h.o) h.o.remove(); }); }
            });

            $('#modalIps').jqm({
                ajax: '<%= Url.Action("BatchNew", "Action") %>',
                trigger: $('.newIps'),
                onShow: function (h) { h.w.slideDown(); },
                onHide: function (h) { h.w.slideUp('medium', function () { if (h.o) h.o.remove(); }); }
            });

            $('.newOwner').click(function () {
                $('#NewOwnerId').val($(this).attr('id'));
            });

            Modal($('#newOwner'), $('.newOwner'));

            $('#submitNewOwners').click(function () {
                $.ajax({
                    type: 'POST',
                    url: '<%= Url.Content("~/Ajax/NewOwners.ashx") %>',
                    data: { value: $('#NewOwners').val(), id: $('#NewOwnerId').val() },
                    success: function (data) {
                        $('#ul__' + $('#NewOwnerId').val().split('__')[1]).append(data);
                        $('#NewOwners').val('');
                        FlashFade($('li.new'), 'green');
                    }
                });
                $('#newOwner').jqmHide();
            });

            var changeCount = 0; //useed to make sure it deletes the right owner if done successively

            $('.deleteOwner').click(function () {
                $.ajax({
                    type: 'POST',
                    url: '<%= Url.Content("~/Ajax/DeleteOwner.ashx") %>',
                    data: { id: $(this).attr('id'), name: $(this).parent().text() },
                    success: function (data) {
                        $(data).slideUp();
                        changeCount++;
                    }
                });
            });


        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadTitle" runat="server">
    ServerInfo
</asp:Content>
