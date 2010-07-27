using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ServerInfo.DomainModel.XmlInterface
{
    public static class XmlSettings
    {
        public class Display 
        {
            public static bool FlipDisplay(string pathToSettingsFile, string key)
            {
                XDocument xDoc = XDocument.Load(pathToSettingsFile);
                XElement property = xDoc.Descendants(key).SingleOrDefault();
                bool value = Convert.ToBoolean(property.Value);
                property.Value = (!value).ToString();
                xDoc.Save(pathToSettingsFile);
                return !value;
            }
        }
    }
}
