using MicroRuleEngine.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Xml;

namespace MicroRuleEngine.Tests
{
    [TestClass]
    public class UpdatePropertyTest
    {
        [TestMethod]
        public void updateorderStatus()
        {
            Order order=ExampleUsage.GetOrder();

            order= Helpers.ChangePropertyValue<Order>(order, new Rule() { MemberName = "Status", TargetValue = "Cancelled" });
            Assert.IsTrue(order.Status == Status.Cancelled);
        }

        [TestMethod]
        public void updateXmlElement()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><CountryCode>USA</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country>" +
                "<Status></Status></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            doc = Helpers.ChangePropertyValue<XmlDocument>(doc, new Rule() { MemberName = "Status", TargetValue = "Cancelled" });
            Assert.IsTrue(doc.GetElementsByTagName("Status").Item(0).InnerText== "Cancelled");
        }

       
    }
}
