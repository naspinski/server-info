using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using System.Xml.Linq;
using ServerInfo.DomainModel.XmlInterface;
using System.DirectoryServices;
using System.Text;
using Naspinski.Utilities;

namespace ServerInfo.DomainModel.Entities
{
    public static class ServerQuery
    {
        public static void UpdateCache(string pathToServerData, string pathToAppParseData, TempDataDictionary tdd)
        {
            XDocument xServers = XDocument.Load(pathToServerData);
            XElement servers = xServers.Descendants("servers").First();
            XDocument xAppParse = XDocument.Load(pathToAppParseData);
            string error = string.Empty;
            string ip;
            List<string> delete = new List<string>();

            IEnumerable<string> dirs = xAppParse.Descendants("directory").Select(x => x.Value);
            IEnumerable<string> dbs_to_ignore = xAppParse.Descendants("db_filename").Select(x => x.Value);
            IEnumerable<string> ignoreDirectories = dirs == null ? new List<string>() : dirs;
            ServerSummary server;

            List<ServerSummary> updatedServers = new List<ServerSummary>();

            foreach(XElement xe in xServers.Descendants("server"))
            {
                try
                {
                    server = new ServerSummary(xe, true);
                    server.Update(ignoreDirectories, dbs_to_ignore);
                    updatedServers.Add(server);
                }
                catch (COMException ex)
                {
                    if (ex.Message.Contains("The RPC server is unavailable"))
                        xe.Attribute("name").Value = "<span class=\"err\">NOT FOUND</span>";
                    error += string.IsNullOrEmpty(error) ? string.Empty : "; ";
                    error += xe.Attribute("ip").Value + " cannot be reached";
                }
                catch (Exception ex) { 
                    throw ex; }
            }
            
            xServers.Descendants("timestamp").First().Value = DateTime.Now.ToString();
            xServers.Descendants("server").Remove();
            servers.Add(updatedServers.Select(x => x.ToXElement()));
            xServers.Save(pathToServerData);

            if (!string.IsNullOrEmpty(error)) throw new COMException(error);
        }

        private static void Update(this ServerSummary server, IEnumerable<string> ignoreDirectories, IEnumerable<string> dbs_to_ignore)
        {
            ManagementScope scope = new ManagementScope(@"\\" + server.Ip + @"\root\cimv2");
            scope.Connect();

            UpdateWindowsInformation(server, scope);
            UpdateApplicationInformation(server, scope, ignoreDirectories, scope.Information(Utilities.WMIQueries.Applications));
            UpdateDatabaseInformation(server, scope, dbs_to_ignore);
            UpdateWebsiteInformation(server);
        }

        public static void UpdateWebsiteInformation(ServerSummary server)
        {
            //StringBuilder sb = new StringBuilder("<br />------------------<br/>");

            //string ip = xe.Attribute("ip").Value;
            List<Website> websites = new List<Website>();
            DirectoryEntry iis = new DirectoryEntry("IIS://" + server.Ip + "/w3svc");
            foreach (DirectoryEntry site in iis.Children)
            {
                try
                {
                    if (!string.IsNullOrWhiteSpace(site.Properties["ServerComment"].Value.ToString()))
                    {
                        Website website = new Website() { Name = site.Properties["ServerComment"].Value.ToString() };
                        try { website.Tcp = site.Properties["ServerBindings"].Value.ToString().ReplacePortNoise(server.Ip).ToNullable<int>(); }
                        catch { } //  no TCP
                        try { website.Ssl = site.Properties["SecureBindings"].Value.ToString().ReplacePortNoise(server.Ip).ToNullable<int>(); }
                        catch { } // no SSL
                        websites.Add(website);
                    }
                    //sb.AppendLine(site.Properties["ServerComment"].Value + " &raquo;");
                    //try { sb.AppendLine(" TCP:" + ); }
                    //catch { }
                    //try { sb.AppendLine(" SSL:" + site.Properties["SecureBindings"].Value.ToString().ReplacePortNoise(ip)); }
                    //catch { }

                    //sb.AppendLine("<br />");
                    //sb.AppendLine("<h3>" + site.Name + "</h3>");
                    //foreach (var v in site.Properties.PropertyNames)

                    //    sb.AppendLine(v.ToString() + " : " + site.Properties[v.ToString()].Value + "<br />");
                }
                catch { } //not a website
            }
            server.Websites = websites;
           // return sb.ToString();
        }

        private static string ReplacePortNoise(this string port, string ip)
        {
            return port.Replace(ip, string.Empty).Replace(":", string.Empty);
        }

        private static void UpdateDatabaseInformation(ServerSummary server, ManagementScope scope, IEnumerable<string> dbs_to_ignore)
        {
            List<string> drives = new List<string>();
            List<string> sql_folders = new List<string>();
            List<string> search_folders = new List<string>();

            foreach(ManagementObject m in scope.Information(Utilities.WMIQueries.Drives))
                drives.Add(m["Caption"].ToString());
            
            foreach (string s in drives)
            {
                foreach (string root in Utilities.PossibleSqlDirectories)
                    search_folders.Add(@"\\" + server.Ip + @"\" + s.Replace(":", "$") + root);
            }

            IEnumerable<string> dbNames = new List<string>();
            foreach (string folder in search_folders)
            {
                if (Directory.Exists(folder))
                    dbNames = dbNames.Union(Directory.GetFiles(folder, "*.mdf", SearchOption.AllDirectories).Select(x => Path.GetFileNameWithoutExtension(x)));
            }
            server.Databases = dbNames.Where(x => !dbs_to_ignore.Contains(x));
        }

        private static void UpdateApplicationInformation(ServerSummary server, ManagementScope scope, IEnumerable<string> ignoreDirectories, ManagementObjectCollection managementObjectCollection)
        {
            server.Applications = GetApplicationParseData(ignoreDirectories, scope.Information(Utilities.WMIQueries.Applications)).Applications;
        }

        private static void UpdateWindowsInformation(ServerSummary server, ManagementScope scope)
        {
            foreach (ManagementObject m in scope.Information(Utilities.WMIQueries.Windows))
            {
                server.Name = m["csname"].ToString();
                server.Os = m["Caption"] + ":" + m["Version"];
            }
        }

        public static ManagementObjectCollection Information(this ManagementScope scope, string searchString)
        {
            //Query system for Operating System information
            ObjectQuery query = new ObjectQuery(searchString);
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query);
            return searcher.Get();
        }
        
        private static ApplicationParseData GetApplicationParseData(IEnumerable<string> ignoreDirectories, ManagementObjectCollection appParseDataSearcher)
        {
            List<string> wmiNames = new List<string>();
            foreach (ManagementObject m in appParseDataSearcher)
                wmiNames.Add(m["Name"].ToString());
            return new ApplicationParseData(ignoreDirectories, wmiNames);
        }
    }
}
