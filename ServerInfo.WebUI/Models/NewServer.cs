using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace ServerInfo.WebUI.Models
{
    public class NewServer
    {
        public string Ip { get; set; }
        public string Owners { get; set; }
        public IEnumerable<string> EnumerateOwners
        {
            get
            {
                if (string.IsNullOrEmpty(Owners))
                    return new List<string>();
                return Owners.Split(new string[] { ";", " ", "," }, StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
            }
        }

        public string this[string propName]
        {
            get
            {
                if (propName == "Address")
                {
                    if (string.IsNullOrEmpty(Ip)) return "required";
                    if (!Regex.IsMatch(Ip, DomainModel.Utilities.IpRegEx)) 
                        return "invalid ip address";
                }
                return null;
            }
        }
        public string Error { get { return null; } }
    }
}