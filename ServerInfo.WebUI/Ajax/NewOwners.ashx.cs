using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerInfo.DomainModel;
using ServerInfo.DomainModel.XmlInterface;

namespace ServerInfo.WebUI.Ajax
{
    /// <summary>
    /// Summary description for NewOwners
    /// </summary>
    public class NewOwners : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string[] split = context.Request.Form["id"].Split(new string[] { "__" }, StringSplitOptions.RemoveEmptyEntries);
                string ip = split[1].Replace("_", ".");
                IEnumerable<string> owners = context.Request.Form["value"].EnumerateSearchString();
                Data.AddOwnersTo(System.Web.HttpContext.Current.Server.MapPath(Settings.DataPaths.Servers), ip, owners);
                foreach (string owner in owners) context.Response.Write("<li class=\"new\">" + owner + "</li>");
            }
            catch (Exception ex) { context.Response.Write("error: " + ex.Message);  }
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