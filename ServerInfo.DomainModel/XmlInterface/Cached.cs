using System.Collections.Generic;
using System.Xml.Linq;
using ServerInfo.DomainModel.Entities;
using System;
using System.Linq;

namespace ServerInfo.DomainModel.XmlInterface
{
    public static class Cached
    {
        public static IEnumerable<ServerSummary> ServerSummaries(XDocument xDoc, string sortBy, string sortDir)
        {
            IEnumerable<XElement> servers;
            List<string> ips = new List<string>();
            sortBy = string.IsNullOrEmpty(sortBy) ? "ip" : sortBy;
            sortDir = string.IsNullOrEmpty(sortDir) ? "up" : sortDir;

            if(sortDir.ToLower().Equals("up")) servers = xDoc.Descendants("server").OrderBy(x => x.Attribute(sortBy).Value);
            else servers = xDoc.Descendants("server").OrderByDescending(x => x.Attribute(sortBy).Value);
            
            return servers.Select(x => new ServerSummary(x));
        }

        public static DateTime TimeStamp(XDocument xDoc)
        {
            return DateTime.Parse(xDoc.Descendants("timestamp").First().Value);
        }
    }
}
