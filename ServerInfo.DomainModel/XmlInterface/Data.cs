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
                xDoc.Descendants("server").Where(x => x.Attribute("ip").Value.Equals(ip)).First().Remove();
                xDoc.Save(pathToServerFile);
                tdd.AddWarning(ip + " removed");
            }
            catch (Exception ex)
            {
                tdd.AddError(ex.Message);
            }
        }
    }
}
