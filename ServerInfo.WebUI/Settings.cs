using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}