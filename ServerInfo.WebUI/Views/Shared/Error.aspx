<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<System.Web.Mvc.HandleErrorInfo>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div id="content">
        <fieldset>
            <legend><i class="error"></i><%= Model.Exception.GetType().ToString() %></legend>
            <div>you found an error</div>
            <div>
                <label>controller</label>
                <%= Html.Encode(Model.ControllerName) %>
            </div>
            <div>
                <label>action</label>
                <%= Html.Encode(Model.ActionName) %>
            </div>
            <div>
                <label>message</label>
                <%= Html.Encode(Model.Exception.Message) %>
                <% if (Model.Exception.InnerException != null) { %> - <%= Html.Encode(Model.Exception.InnerException.Message) %><% } %>
            </div>
        </fieldset>    
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadTitle" runat="server">
    welcome to error town
</asp:Content>
