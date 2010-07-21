using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ServerInfo.WebUI.Models;
using ServerInfo.DomainModel.XmlInterface;
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


        // POST: /Action/New
        [HttpPost]
        public ActionResult New(NewServer newServer)
        {
            try
            {
                Data.NewServerSummary(newServer.Ip, newServer.EnumerateOwners, Server.MapPath(Settings.DataPaths.Servers));
                TempData.AddSuccess(newServer.Ip + " added to system");
            }
            catch(Exception ex)
            {
                TempData.AddError(ex.Message);
            }
            return RedirectToAction("Refresh", "Home");
        }
        
        //
        // GET: /Action/Edit/5
 
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Action/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Action/Delete/5
 
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Action/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
 
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
