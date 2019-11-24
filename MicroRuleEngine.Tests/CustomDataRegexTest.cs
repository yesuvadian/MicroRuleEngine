using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Xml;

namespace MicroRuleEngine.Tests
{
    [TestClass]
    public class CustomDataRegexTest
    {
        [TestMethod]
        public void XmlregexpassProperties2()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><CountryCode>USA</CountryCode>" +
                "<State><StateCode>NY</StateCode></State><SSN>123-12-3345</SSN></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = new Rule() {MemberName="SSN",Operator= "IsMatch", TargetValue= "(^\\d{3}-?\\d{2}-?\\d{4}$|^XXX-XX-XXXX$)" } &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void XmlRegexfailProperties2()
        {
            //Order order = ExampleUsage.GetOrder();
            string xml = "<root><Country><CountryCode>USA</CountryCode>" +
                "<State><StateCode>NY</StateCode></State><SSN>123-123-3345</SSN></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            Rule rule = new Rule() { MemberName = "SSN", Operator = "IsMatch", TargetValue = "(^\\d{3}-?\\d{2}-?\\d{4}$|^XXX-XX-XXXX$)" } &
                Rule.Create("Country.State.StateCode", mreOperator.Equal, "NY");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<XmlDocument>(rule);
            bool passes = compiledRule(doc, "");
            Assert.IsFalse(passes);

        }

        [TestMethod]
        public void JsonregexPass()
        {
            //Order order = ExampleUsage.GetOrder();
            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = new Rule() { MemberName = "PageNumber", Operator = "IsMatch", TargetValue = "(?<=\\s|^)\\d+(?=\\s|$)" } &
            Rule.Create("RecordId", mreOperator.Equal, "788");
            
            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void JsonregexPassFail()
        {
            //Order order = ExampleUsage.GetOrder();
            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90-1\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = new Rule() { MemberName = "PageNumber", Operator = "IsMatch", TargetValue = "(?<=\\s|^)\\d+(?=\\s|$)" } &
            Rule.Create("RecordId", mreOperator.Equal, "788");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsFalse(passes);

        }
    }
}
