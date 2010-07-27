<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <fieldset>

        <legend>Testing</legend>
    
        <div id="response"></div>

        <% using (Ajax.BeginForm("Test", null, new AjaxOptions { UpdateTargetId = "response" }, new { id="frm" }))
           { %>
            <div>
                <%= Html.Label("ABC")%>
                <%= Html.TextBox("ABC")%>
            </div>
            <input type="submit" value="save" id="heyo" />
        <% } %>

    </fieldset>

    <script type="text/javascript">
        $().ready(function () {
            var form = $('#frm');
            $('#heyo').click(function (e) {
            e.s
                $.post(form.attr("action"), form.serialize(), function (data) {
                });
                return false;
            });
            $('#response').html('heyo');
        });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadTitle" runat="server">
</asp:Content>
