using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerInfo.DomainModel.XmlInterface;
using System.Web.Mvc;

namespace ServerInfo.WebUI
{
    public static class DalBridge
    {
        public static class ServerSummaries
        {
            public static void NewIps(IEnumerable<string> ips)
            {
                if (ips != null && ips.Count() > 0)
                    Data.NewServerSummaries(ips, System.Web.HttpContext.Current.Server.MapPath(Settings.DataPaths.Servers));
            }

            public static void NewIp(string ip, IEnumerable<string> owners)
            {
                Data.NewServerSummary(ip, owners, System.Web.HttpContext.Current.Server.MapPath(Settings.DataPaths.Servers));
            }
        }
    }
}