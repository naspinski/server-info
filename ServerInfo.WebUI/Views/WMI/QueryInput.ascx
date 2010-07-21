<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

    <% using (Html.BeginForm("Query", "WMI"))
        { %>
        <fieldset class="clear">
            <legend><i class="arrow_blue"></i>Manual WMI Query</legend>
            <div>
                <label for="ip">IP to Query</label>
                <%= Html.TextBox("ip", TempData["ip"])%>
            </div>
            <div>
                <label for="query">WQL Query</label>
                <%= Html.TextBox("query", TempData["query"])%>
            </div>
            <div class="align_center">
                <input type="submit" value="submit" />
            </div>
        </fieldset>
    <% } %>