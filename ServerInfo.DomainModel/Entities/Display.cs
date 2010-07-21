using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using Naspinski.Utilities;

namespace ServerInfo.DomainModel.Entities
{
    public class Display
    {
        public bool Name { get; set; }
        public bool Ip { get; set; }
        public bool Os { get; set; }
        public bool Owners { get; set; }
        public bool Applications { get; set; }
        public bool Databases { get; set; }
        public bool Websites { get; set; }
        public IEnumerable<string> ActivePropertyNames 
        { // returns an IEnumerable of the Property Names that are True
            get
            {
                return this.GetType().GetProperties().Where(x => x.PropertyType == typeof(Boolean))
                    .Where(x => Convert.ToBoolean(x.GetValue(this, null)) == true)
                    .Select(x => x.Name);
            }
        }

        public Display(XDocument xDoc)
        {
            foreach (XElement xe in xDoc.Descendants("display").First().Descendants())
                this.ChangePropertyValue(xe.Name.ToString(), Convert.ToBoolean(xe.Value));
        }
    }
}
