using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace ServerInfo.DomainModel.Entities
{
    public class ApplicationParseData
    {
        private IEnumerable<string> IgnoreDirectories { get; set; }
        private string Delimiter { get { return ":"; } }
        public IEnumerable<string> Applications { get; set; }

        public ApplicationParseData(IEnumerable<string> ignoreDirectories, IEnumerable<string> WmiNames)
        {
            IgnoreDirectories = ignoreDirectories;
            
            // VERY INEFFICIENT //
            // find a better way to do this!
            var userApps = WmiNames
                .Where(x => x.Split(new string[] {@"\"}, StringSplitOptions.RemoveEmptyEntries).Count() > 2 &&
                    x.Split(new string[] { @"\" }, StringSplitOptions.RemoveEmptyEntries).Count() < 4)
                .Select(x => x.Substring(Utilities.WmiProgramGroup.Length + 1, (x.Length - 1) - Utilities.WmiProgramGroup.Length).Replace(Utilities.StartMenuPrograms, string.Empty));
            bool add;
            List<string> applications = new List<string>();
            foreach (string a in userApps)
            {
                add = true;
                foreach (string i in IgnoreDirectories.Select(x => x.ToLower()))
                {
                    if (i.Length <= a.Length && a.ToLower().Substring(0, i.Length).Equals(i))
                        add = false;
                }
                if (add) applications.Add(a);
            }
            Applications = applications.OrderBy(x => x);
        }
    }
}
