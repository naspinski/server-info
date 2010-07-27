using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;

namespace ServerInfo.WebUI.HtmlHelpers
{
    public static class WMIInfoHelper
    {
        public static string WMIInfo(this HtmlHelper html)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<div>WQL is a special WMI Query language - think SQL for WMI</div>");
            sb.AppendLine("<br />");
            sb.AppendLine("<ul>");
            sb.AppendLine("    <li> - <a href=\"http://msdn.microsoft.com/en-us/library/aa394606(VS.85).aspx\">WQL (SQL for WMI)</a></li>");
            sb.AppendLine("    <li> - <a href=\"http://msdn.microsoft.com/en-us/library/aa394572(v=VS.85).aspx\">WMI Reference</a></li>");
            sb.AppendLine("<ul>");
            sb.AppendLine("<br />ex: <b>SELECT * FROM Win32_LogicalDisk</b> will get all the drive information for the specified ip");
            return sb.ToString();
        }
    }
}