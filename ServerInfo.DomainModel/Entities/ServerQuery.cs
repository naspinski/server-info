using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Web.Mvc;
using ServerInfo.DomainModel.XmlInterface;
using System.IO;
using Microsoft.SqlServer;

namespace ServerInfo.DomainModel.Entities
{
    public static class ServerQuery
    {
        public static void UpdateCache(string pathToServerData, string pathToAppParseData, TempDataDictionary tdd)
        {
            XDocument xServers = XDocument.Load(pathToServerData);
            XDocument xAppParse = XDocument.Load(pathToAppParseData);
            string error = string.Empty;
            string ip;
            List<string> ips = new List<string>();
            List<string> delete = new List<string>();
            var dirs = xAppParse.Descendants("directory").Select(x => x.Value);
            IEnumerable<string> ignoreDirectories = dirs == null ? new List<string>() : dirs;

            foreach(XElement xe in xServers.Descendants("server"))
            {
                try
                {
                    ip = xe.Attribute("ip").Value;
                    if (ips.Contains(ip)) delete.Add(ip);
                    else ips.Add(ip);
                    xe.Update(ignoreDirectories);
                }
                catch (COMException ex)
                {
                    if (ex.Message.Contains("The RPC server is unavailable"))
                        xe.Attribute("name").Value = "<span class=\"err\">NOT FOUND</span>";
                    error += string.IsNullOrEmpty(error) ? string.Empty : "; ";
                    error += xe.Attribute("ip").Value + " cannot be reached";
                }
                catch (Exception ex) { throw ex; }
            }

            xServers.Descendants("timestamp").First().Value = DateTime.Now.ToString();
            xServers.Save(pathToServerData);

            foreach (string s in delete)
                Data.DeleteServerSummary(s, tdd, pathToServerData);

            if (!string.IsNullOrEmpty(error)) throw new COMException(error);
        }

        private static void Update(this XElement xe, IEnumerable<string> ignoreDirectories)
        {
            ManagementScope scope = new ManagementScope(@"\\" + xe.Attribute("ip").Value + @"\root\cimv2");
            scope.Connect();

            UpdateWindowsInformation(xe, scope);
            UpdateApplicationInformation(xe, scope, ignoreDirectories, scope.Information(Utilities.WMIQueries.Applications));
            UpdateDatabaseInformation(xe, scope);
        }

        private static void UpdateDatabaseInformation(XElement xe, ManagementScope scope)
        {
            string ip = xe.Attribute("ip").Value;
            List<string> drives = new List<string>();
            List<string> sql_folders = new List<string>();
            List<string> search_folders = new List<string>();
            XElement dbs = xe.Descendants("databases").First();

            foreach(ManagementObject m in scope.Information(Utilities.WMIQueries.Drives))
                drives.Add(m["Caption"].ToString());
            
            foreach (string s in drives)
            {
                foreach (string root in Utilities.PossibleSqlDirectories)
                    search_folders.Add(@"\\" + ip + @"\" + s.Replace(":", "$") + root);
            }

            //var asdf = Directory.GetFiles(search_folders.First(), "*.mdf", SearchOption.AllDirectories);
            //bool asd = true;

        }

        private static void UpdateApplicationInformation(XElement xe, ManagementScope scope, IEnumerable<string> ignoreDirectories, ManagementObjectCollection managementObjectCollection)
        {
            XElement apps = xe.Descendants("applications").First();
            apps.RemoveAll();
            ApplicationParseData apd = GetApplicationParseData(ignoreDirectories, scope.Information(Utilities.WMIQueries.Applications));
            foreach (string s in apd.Applications)
                apps.Add(new XElement("application", s));
        }

        private static void UpdateWindowsInformation(XElement xe, ManagementScope scope)
        {
            foreach (ManagementObject m in scope.Information(Utilities.WMIQueries.Windows))
            {
                xe.Attribute("name").Value = m["csname"].ToString();
                xe.Attribute("os").Value = m["Caption"] + ":" + m["Version"];
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
