<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage<ServerInfo.WebUI.Models.NewServer>" %>

    <div>
        <% using (Html.BeginForm("New", "Action"))
           { %>
            <div>
                <label for="Ip"><%= Html.ValidationMessage("Ip") %>New IP</label>
                <%= Html.TextBox("Ip") %>
            </div>
            <div>
                <label for="Ip"><%= Html.ValidationMessage("Owners") %>Owners</label>
                <%= Html.TextArea("Owners") %>
            </div>
            <div class="align_center">
                <input type="submit" value="submit" />
                <input type="button" value="cancel" class="jqmClose" />
            </div>
        <% } %>
    </div>
