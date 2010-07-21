using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ServerInfo.DomainModel.Entities;
using System.Management;

namespace ServerInfo.Tests.DomainModel
{
    [TestFixture]
    public class ServerQueryTests
    {
        string testserver = "172.31.30.15";

        [Test]
        public void Query_works()
        {            
            ManagementScope scope = new ManagementScope(@"\\" + testserver + @"\root\cimv2");
            scope.Connect();
            foreach (ManagementObject m in scope.Information("SELECT * FROM Win32_OperatingSystem"))
            {
                Console.WriteLine(m["Name"]);
            }
            foreach (ManagementObject m in scope.Information("SELECT Name FROM Win32_ProgramGroup"))
            {
                Console.WriteLine(m["Name"]);
            }
        }
    }
}
