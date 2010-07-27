using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerInfo.DomainModel.XmlInterface;

namespace ServerInfo.WebUI.Ajax
{
    /// <summary>
    /// Summary description for DeleteOwner
    /// </summary>
    public class DeleteOwner : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string[] split = context.Request.Form["id"].Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries);
                string ip = split[1].Replace("_", ".");
                int number = Convert.ToInt32(split[2]);
                string name = context.Request.Form["name"].Replace("[X]",string.Empty);
                Data.RemoveOwnerFrom(System.Web.HttpContext.Current.Server.MapPath(Settings.DataPaths.Servers), ip, name);
                context.Response.Write("#li__" + ip.Replace(".","_") + "__" + number);
            }
            catch (Exception ex) { context.Response.Write("error: " + ex.Message); }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}