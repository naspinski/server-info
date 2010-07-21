using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using ServerInfo.DomainModel.Entities;
using ServerInfo.DomainModel.XmlInterface;
using System.Xml.Linq;

namespace ServerInfo.Tests.DomainModel
{
    [TestFixture]
    public class DisplayTests
    {
        Display d;

        [SetUp]
        public void Setup()
        {
            d = new Display(XDocument.Load(@"..\..\Data\settings.xml"));
        }

        [Test]
        public void Display_intialization_works()
        {
            Assert.AreEqual(d.Name, true);
            Assert.AreEqual(d.Websites, false);
        }

        [Test]
        public void ActivePropertyNames_works()
        {
            Assert.AreEqual(5, d.ActivePropertyNames.Count());
        }
    }
}
