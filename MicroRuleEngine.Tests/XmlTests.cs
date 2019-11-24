using System;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MicroRuleEngine.Tests
{
    [TestClass]
    public class XmlTests
    {
        [TestMethod]
        public void XmlProperties2()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>1</Id><CountryCode>USA</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.Create("Country.CountryCode", mreOperator.Equal, "USA") &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void XmlNumberequlTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>1</Id><CountryCode>USA</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.Create("Country.Id", mreOperator.Equal, 1) &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void XmlEndswithTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>1</Id><CountryCode>USAmerica</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.MethodOnChild("Country.CountryCode", "EndsWith", "America") &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void XmlGreaterThanTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>1</Id><CountryCode>USAmerica</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.Create("Country.Id", mreOperator.GreaterThanOrEqual, 1) &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void XmlGreaterThanFailTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>0</Id><CountryCode>USAmerica</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.Create("Country.Id", mreOperator.GreaterThanOrEqual, 1) &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsFalse(passes);

        }

        [TestMethod]
        public void XmlLessthanFailTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>2</Id><CountryCode>USAmerica</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.Create("Country.Id", mreOperator.LessThanOrEqual, 1) &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsFalse(passes);

        }

        [TestMethod]
        public void XmlLessthanFailstringTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>2</Id><CountryCode>USAmerica</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.Create("Country.Id", mreOperator.LessThanOrEqual, 1) &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsFalse(passes);

        }

        [TestMethod]
        public void XmlLessthanPassTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>0</Id><CountryCode>USAmerica</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.Create("Country.Id", mreOperator.LessThanOrEqual, 1) &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void XmlApplysucessandfailRuleTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><Id>1</Id><CountryCode>USAmerica</CountryCode>" +
                "<State><StateCode>NY</StateCode></State></Country><Status></Status></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = Rule.MethodOnChild("Country.CountryCode", "EndsWith", "India") &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "TN");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            var succesrule = Rule.Create("Status", mreOperator.Equal, "Success");
            var failurerule = Rule.Create("Status", mreOperator.Equal, "Failed");

          
            bool passes = compiledRule(doc, "");
            if(passes)
            {
                Helpers.ChangePropertyValue<XmlDocument>(doc, succesrule);
            }
            else
            {
                Helpers.ChangePropertyValue<XmlDocument>(doc, failurerule);
            }

            Assert.IsFalse(passes);

        }

    }
}
