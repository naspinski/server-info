using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using ServerInfo.DomainModel;
using ServerInfo.DomainModel.XmlInterface;

namespace ServerInfo.WebUI
{
    public static class Settings
    {
        public class DataPaths
        {
            public static string Settings { get { return "~/App_Data/settings.xml"; } }
            public static string Servers { get { return "~/App_Data/servers.xml"; } }
            public static string ApplicationParseInfo { get { return "~/App_Data/application_parse_info.xml"; } } 
        }

        public static class Display
        {
            public static bool Flip(string key)
            {
                return XmlSettings.Display.FlipDisplay(System.Web.HttpContext.Current.Server.MapPath(DataPaths.Settings), key);
            }
        }

        public static class ServerSummaries
        {
            public static void NewIps(IEnumerable<string> ips)
            {
                if(ips != null && ips.Count() > 0)
                    Data.NewServerSummaries(ips, System.Web.HttpContext.Current.Server.MapPath(DataPaths.Servers));
            }
        }
    }
}