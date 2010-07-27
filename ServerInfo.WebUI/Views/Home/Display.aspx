<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ServerInfo.DomainModel.Entities.Display>" %>
<%@ Import Namespace="ServerInfo.WebUI.HtmlHelpers" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="content">
        <fieldset class="clear">
            <legend>Display Options</legend>
            <div class="third">
                <span class="check_left"><%= Html.CheckBoxFor(x => x.Name) %></span>
                <%= Html.LabelFor(x => x.Name) %>
            </div>
            <div class="third">
                <span class="check_left"><%= Html.CheckBoxFor(x => x.Ip) %></span>
                <%= Html.LabelFor(x => x.Ip) %>
            </div>
            <div class="third">
                <span class="check_left"><%= Html.CheckBoxFor(x => x.Owners) %></span>
                <%= Html.LabelFor(x => x.Owners) %>
            </div>
            <div class="third">
                <span class="check_left"><%= Html.CheckBoxFor(x => x.Os) %></span>
                <%= Html.LabelFor(x => x.Os) %>
            </div>
            <div class="third">
                <span class="check_left"><%= Html.CheckBoxFor(x => x.Applications) %></span>
                <%= Html.LabelFor(x => x.Applications) %>
            </div>
            <div class="third">
                <span class="check_left"><%= Html.CheckBoxFor(x => x.Databases) %></span>
                <%= Html.LabelFor(x => x.Databases) %>
            </div>
            <div class="third">
                <span class="check_left"><%= Html.CheckBoxFor(x => x.Websites) %></span>
                <%= Html.LabelFor(x => x.Websites) %>
            </div>
            <div class="clear"></div>
        </fieldset>
    </div>
    <div id="sidebar">
        <%= Html.Navigation(Request.RequestContext.RouteData.Values) %>
        <fieldset class="box">
            <legend>info</legend>
            <div>This is what will be displayed on the home page.</div>
            <br />
            <ul>
                <li> - Changes are automatically saved.</li>
                <li> - Hidden information will not be lost.</li>
                <li> - Hidden information will still be updated.</li>
            </ul>
        </fieldset>
    </div>
    
    <script type="text/javascript">
        $().ready(function () {
            $('input[type=checkbox]').change(function () {
                var key = $(this).attr('id');
                $.ajax({
                    type: 'POST',
                    url: '<%= Url.Content("~/Ajax/FlipDisplaySetting.ashx") %>',
                    data: { key: key },
                    success: function (data) {
                        FlashFade($('label[for="' + key + '"]'), 'green');
                    }
                });
            });
        })
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadTitle" runat="server">
    Display Options
</asp:Content>
