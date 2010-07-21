using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Management;
using System.Text.RegularExpressions;
using Naspinski.Utilities;

namespace ServerInfo.DomainModel.Entities
{
    public class ServerSummary
    {
        public string Name { get; set; }
        public string Ip { get; set; }
        public string Os { get; set; }
        public IEnumerable<string> Owners { get; set; }
        public IEnumerable<string> Applications { get; set; }
        public IEnumerable<string> Databases { get; set; }
        public IEnumerable<Website> Websites { get; set; }
        public Dictionary<string, string> Strings
        {
            get
            {
                Dictionary<string, string> strings = new Dictionary<string, string>()
                { {"Name", Name}, {"Ip", Ip}, {"Os", Os} };
                strings.Add("Owners", MakeList(Owners));
                strings.Add("Applications", MakeList(Applications));
                strings.Add("Databases", MakeList(Databases));
                strings.Add("Websites", MakeList(null, Websites));
                return strings;
            }
        }

        public ServerSummary(XElement xe) { MakeServerSummaryFromXElement(xe, false); }
        public ServerSummary(XElement xe, bool cleanOutEverything) { MakeServerSummaryFromXElement(xe, true); }
        public ServerSummary(string ip, IEnumerable<string> owners)
        {
            if (!Regex.IsMatch(ip, Utilities.IpRegEx))
                throw new Exception(ip + " is not a valid ip address");
            Ip = ip;
            Owners = owners;

            Name = string.Empty;
            Os = String.Empty;
            Applications = new List<string>();
            Databases = new List<string>();
            Websites = new List<Website>();
        }

        public XElement ToXElement()
        {
            //Websites = new List<Website>();
            return new XElement("server",
                new XAttribute("ip", Ip),
                new XAttribute("name", Name),
                new XAttribute("os", Os),
                new XElement("owners", Owners.Select(x => new XElement("owner", x))),
                new XElement("applications", Applications.Select(x => new XElement("application", x))),
                new XElement("databases", Databases.Select(x => new XElement("database", x))),
                new XElement("websites", Websites.Select(x => new XElement("website", new XAttribute("tcp", (x.Tcp == null ? "" : x.Tcp.ToString())), new XAttribute("ssl", (x.Ssl == null ? "" : x.Ssl.ToString())), x.Name)))
                //new XElement("websites", Websites.Select(x => new XElement("website", new XAttribute("tcp", "90"), new XAttribute("ssl", "443"), x.Name)))
                );
        }

        public void SaveToFile(string pathToXmlFile)
        {
            XDocument xDoc = XDocument.Load(pathToXmlFile);
            if (xDoc.Descendants("server").Where(x => x.Attribute("ip").Value.Equals(Ip)).Count() > 0)
                throw new Exception(Ip + " is already in the system");
            xDoc.Descendants("servers").First().Add(this.ToXElement());
            xDoc.Save(pathToXmlFile);
        }

        private string MakeList(IEnumerable<string> strings, IEnumerable<Website> websites = null)
        {
            string temp = "<ul class=\"mini\">";
            if (websites == null)
                { foreach (string s in strings) temp += "<li>" + s + "</li>"; }
            else
            { 
                foreach (Website w in websites)
                    temp += "<li>" + w.Name + "<span class=\"ports\">" + 
                        (w.Tcp != null 
                            ? " <a href=\"http://" + Ip + ":" + w.Tcp + "\">TCP:" + w.Tcp + "</a>"
                            : string.Empty) + 
                        (w.Ssl != null 
                            ? " <a href=\"https://" + Ip + ":" + w.Ssl + "\">SSL:" + w.Ssl + "</a>"
                            : string.Empty) +
                        "</span></li>"; 
            }
            temp += "</ul>";
            return temp;
        }

        private void MakeServerSummaryFromXElement(XElement xe, bool makeClean)
        {
            if (!makeClean)
            { 
                IEnumerable<string> applications = xe.Descendants("application").Select(x => x.Value);
                IEnumerable<string> databases = xe.Descendants("database").Select(x => x.Value);
                IEnumerable<Website> websites = xe.Descendants("website")
                    .Select(x => new Website() 
                    { 
                        Name = x.Value, 
                        //Tcp = string.IsNullOrWhiteSpace(x.Attribute("tcp").Value) ? null : x.Attribute("tcp").Value.ToNullable<int>();
                        Tcp = x.Attribute("tcp").Value.ToNullable<int>(),
                        Ssl = x.Attribute("ssl").Value.ToNullable<int>()
                    });
                Applications = applications;
                Databases = databases;
                Websites = websites;
                Os = xe.Attribute("os").Value;
                Name = xe.Attribute("name").Value;
            }
            IEnumerable<string> owners = xe.Descendants("owner").Select(x => x.Value);
            Ip = xe.Attribute("ip").Value;
            Owners = owners;
        }
    }
}
