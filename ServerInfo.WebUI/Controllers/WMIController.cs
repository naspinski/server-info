using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Management;
using ServerInfo.DomainModel;
using ServerInfo.DomainModel.Entities;
using System.Text.RegularExpressions;

namespace ServerInfo.WebUI.Controllers
{
    public class WMIController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Query()
        {
            List<string> strings = new List<string>();

            string ip = Request.Form["ip"];
            string query = Request.Form["query"];
            if (!Regex.IsMatch(ip, Utilities.IpRegEx)) TempData["error"] = "'" + ip + "' is not a valid IP address";
            else
            {
                try
                {
                    ManagementScope scope = new ManagementScope(@"\\" + ip + @"\root\cimv2");
                    scope.Connect();

                    strings.Add("        <legend><i class=\"success\"></i>" + query + "</legend>");
                    strings.Add("        <ul>");
                    foreach (ManagementObject m in scope.Information(query))
                    {
                        foreach (var v in m.Properties)
                            strings.Add("            <li>" + v.Name + " : " + m[v.Name] + "</li>");
                        strings.Add("            <li style=\"line-height:.5em;\">-</li>");
                    }
                    strings.Add("        </ul>");
                    TempData["ip"] = ip;
                    TempData["query"] = query;
                }
                catch (Exception ex) { TempData["error"] = ex.Message; }
            }
            return View(strings);
        }

    }
}
