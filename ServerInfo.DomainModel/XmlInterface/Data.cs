using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerInfo.DomainModel.Entities;
using System.Web.Mvc;
using System.Xml.Linq;

namespace ServerInfo.DomainModel.XmlInterface
{
    public static class Data
    {
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

        public static void AddOwnerTo(string pathToServerFile, string ip, string owner)
        {

        }

        public static void AddOwner(this XElement xe, string owner)
        {
            if (xe.Descendants("owner").Where(x => x.Value.ToLower().Equals(owner.ToLower())).Count() > 0)
                throw new InvalidOperationException(owner + " alreaday exists");

            xe.Add(new XElement("owner", owner));
        }

        public static XElement GetOwner(this XElement xe, string owner)
        {
            return xe.Descendants("owner").Where(x => x.Value.ToLower().Equals(owner.ToLower())).First();
        }
    }
}
