using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerInfo.DomainModel.Entities;
using System.Web.Mvc;
using System.Xml.Linq;
using System.Text.RegularExpressions;

namespace ServerInfo.DomainModel.XmlInterface
{
    public static class Data
    {
        public static void NewServerSummaries(IEnumerable<string> ips, string pathToServerFile)
        {
            foreach (string ip in ips.Distinct().Where(x => !string.IsNullOrWhiteSpace(x) && Regex.IsMatch(x, Utilities.IpRegEx)))
                NewServerSummary(ip, new List<string>(), pathToServerFile);
        }

        public static void NewServerSummary(string ip, IEnumerable<string> owners, string pathToServerFile)
        {
            ServerSummary server = new ServerSummary(ip, owners);
            server.SaveToFile(pathToServerFile);
        }

        public static void DeleteServerSummary(string ip, TempDataDictionary tdd, string pathToServerFile)
        {
            try
            {
                XDocument xDoc = XDocument.Load(pathToServerFile);
                xDoc.GetServerXElement(ip).Remove();
                xDoc.Save(pathToServerFile);
                tdd.AddWarning(ip + " removed");
            }
            catch (Exception ex)
            {
                tdd.AddError(ex.Message);
            }
        }

        public static XElement GetServerXElement(this XDocument xDoc, string ip)
        {
            return xDoc.Descendants("server").Where(x => x.Attribute("ip").Value.Equals(ip)).First();
        }

        public static void AddOwnersTo(string pathToServerFile, string ip, IEnumerable<string> owners)
        {
            XDocument xDoc = XDocument.Load(pathToServerFile);
            xDoc.GetServerXElement(ip).Descendants("owners").First().AddOwners(owners);
            xDoc.Save(pathToServerFile);
        }

        public static void RemoveOwnerFrom(string pathToServerFile, string ip, string name)
        {
            XDocument xDoc = XDocument.Load(pathToServerFile);
            xDoc.GetServerXElement(ip).RemoveOwner(name);
            xDoc.Save(pathToServerFile);
        }

        private static void RemoveOwner(this XElement xe, string name)
        { xe.Descendants("owner").Where(x => x.Value.Equals(name)).First().Remove(); }


        private static void AddOwners(this XElement xe, IEnumerable<string> owners)
        { foreach (string s in owners) xe.AddOwner(s); }

        private static void AddOwner(this XElement xe, string owner)
        {
            if (xe.Descendants("owner").Where(x => x.Value.ToLower().Equals(owner.ToLower())).Count() > 0)
                throw new InvalidOperationException(owner + " alreaday exists");

            xe.Add(new XElement("owner", owner));
        }

        private static XElement GetOwner(this XElement xe, string owner)
        { return xe.Descendants("owner").Where(x => x.Value.ToLower().Equals(owner.ToLower())).First(); }
    }
}
