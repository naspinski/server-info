using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Text;
using System.Web.Routing;

namespace ServerInfo.WebUI.HtmlHelpers
{
    public static class NavigationHelper
    {
        public class NavLink
        {
            public string Text { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public NavLink(){}
        }

        public static IEnumerable<NavLink> NavLinks { 
            get 
            {
                return new List<NavLink>() {
                    new NavLink() { Text = "home", Controller = "Home", Action = "Index" },
                    new NavLink() { Text = "display options", Controller = "Home", Action = "Display" },
                    new NavLink() { Text = "wmi query", Controller = "WMI", Action = "Index" }
                };
            }
        }

        public static string Navigation(this HtmlHelper html, RouteValueDictionary routeData)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<fieldset class=\"box\">");
            sb.AppendLine("    <legend>navigation</legend>");
            sb.AppendLine("    <ul class=\"list_vertical\">");
            
            foreach (NavLink l in NavLinks)
                sb.AppendLine("                <li>" + html.RouteLink(l.Text, new { action = l.Action, controller = l.Controller }, new { @class = (l.Controller.Equals(routeData["controller"].ToString()) && l.Action.Equals(routeData["action"].ToString()) ? "current" : "") }).ToHtmlString() + "</li>");

            sb.AppendLine("    </ul>");
            sb.AppendLine("</fieldset>");
            return sb.ToString();
        }
    }
}