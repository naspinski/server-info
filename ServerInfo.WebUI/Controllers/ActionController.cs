using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServerInfo.WebUI.Models;
using ServerInfo.DomainModel;

namespace ServerInfo.WebUI.Controllers
{
    [HandleError]
    public class ActionController : Controller
    {
        // GET: /Action/New
        public ActionResult New()
        {
            return View();
        }

        public ActionResult BatchNew()
        {
            return View();
        }


        // POST: /Action/New
        [HttpPost]
        public ActionResult New(NewServer newServer)
        {
            try
            {
                DalBridge.ServerSummaries.NewIp(newServer.Ip, newServer.EnumerateOwners);
                TempData.AddSuccess(newServer.Ip + " added to system");
            }
            catch(Exception ex)
            {
                TempData.AddError(ex.Message);
            }
            return RedirectToAction("Refresh", "Home");
        }
        
        [HttpPost]
        public ActionResult BatchNew(FormCollection form)
        {
            try
            {
                DalBridge.ServerSummaries.NewIps(form["Ips"].EnumerateSearchString(true));
                TempData.AddSuccess("new ips successfully added to system");
            }
            catch (Exception ex)
            {
                TempData.AddError(ex.Message);
            }
            return RedirectToAction("Refresh", "Home");
        }
    }
}
