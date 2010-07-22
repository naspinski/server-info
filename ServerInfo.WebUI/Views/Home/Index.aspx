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

    <div id="test"></div>

    <div id="modal" class="jqmWindow">loading...</div>
    
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
                ajax: '/Action/New',
                trigger: $('#aNew'),
                onShow: function (h) { h.w.slideDown(); },
                onHide: function (h) { h.w.slideUp('medium', function () { if (h.o) h.o.remove(); }); }
            });

            $('.newOwner').each(function () {
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
                    }
                });
                $('#newOwner').jqmHide();
            });

            var changeCount = 0; //useed to make sure it deletes the right owner if done successively

            $('.deleteOwner').click(function () {
                $.ajax({
                    type: 'POST',
                    url: '<%= Url.Content("~/Ajax/DeleteOwner.ashx") %>',
                    data: { id: $(this).attr('id'), changeCount: changeCount  },
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
