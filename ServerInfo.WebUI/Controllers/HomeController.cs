using System.Collections.Generic;
using System.Web.Mvc;
using ServerInfo.DomainModel.XmlInterface;
using ServerInfo.DomainModel.Entities;
using System.Xml.Linq;
using System;
using System.Linq;
using ServerInfo.WebUI.Models;
using ServerInfo.DomainModel;

namespace ServerInfo.WebUI.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        // GET: /Home/
        public ViewResult Index(string SortBy, string SortDir)
        {
            var view = new DisplayAndServers(Server.MapPath(Settings.DataPaths.Servers), Server.MapPath(Settings.DataPaths.Settings), SortBy, SortDir);
            if (view.Servers.Count() == 0) TempData.AddWarning("no servers in the system");
            return View(view);
        }

        // GET: /Delete/{Ip}
        public ActionResult Delete(string Ip)
        {
            Data.DeleteServerSummary(Ip, TempData, Server.MapPath(Settings.DataPaths.Servers));
            return Refresh();
        }

        // GET: /Refresh
        public ActionResult Refresh()
        {
            try
            {
                ServerQuery.UpdateCache(Server.MapPath(Settings.DataPaths.Servers), Server.MapPath(Settings.DataPaths.ApplicationParseInfo), TempData);
            }
            catch (Exception ex)
            {
                TempData.AddError(ex.Message);
            }
            return RedirectToAction("Index");
        }
    }
}
