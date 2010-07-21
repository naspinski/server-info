using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServerInfo.DomainModel.Entities;
using ServerInfo.DomainModel.XmlInterface;
using System.Xml.Linq;

namespace ServerInfo.WebUI.Models
{
    public class DisplayAndServers
    {
        public IEnumerable<ServerSummary> Servers { get; set; }
        public string SortBy { get; set; }
        public string SortDir { get; set; }
        public Display Display { get; set; }
        public DateTime Timestamp { get; set; }
        public IEnumerable<string> Sorters { get { return new List<string>() { "Ip", "Name", "Os" }; } }

        public DisplayAndServers(string serverXmlPath, string settingsXmlPath, string sortBy, string sortDir)
        {
            SortBy = sortBy.ToLower();
            SortDir = sortDir.ToLower();
            XDocument xDoc = XDocument.Load(serverXmlPath);
            Servers = Cached.ServerSummaries(xDoc, SortBy, sortDir);
            Timestamp = Cached.TimeStamp(xDoc);
            xDoc = XDocument.Load(settingsXmlPath);
            Display = new Display(xDoc);
        }
    }
}