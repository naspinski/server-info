<%@ Page Title="" Language="C#" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


    <div>
        <% using (Html.BeginForm("BatchNew", "Action"))
           { %>
            <div>
                <label for="Ips">New IPs</label>
                <%= Html.TextArea("Ips") %>
            </div>
            <div class="align_center">
                <input type="submit" value="submit" />
                <input type="button" value="cancel" class="jqmClose" />
            </div>
        <% } %>
    </div>
