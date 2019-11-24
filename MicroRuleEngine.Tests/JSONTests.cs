using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MicroRuleEngine.Tests
{
    [TestClass]
   public class JSONTests
    {
        [TestMethod]
        public void JSONPropertiesTrue()
        {
            //Order order = ExampleUsage.GetOrder();
            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("RecordId", mreOperator.Equal, "788") &
                Rule.Create("PageNumber", mreOperator.Equal, "90");

            MRE engine = new MRE();
            var compiledRule = engine.CompileJSONRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void JSONNestedPropertiesPass()
        {

            string json = ReadfromResource("json2.json");
            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("config.rxstatus", mreOperator.Equal, "Pending") &
                Rule.Create("config.rxstatus", mreOperator.Equal, "Pending");
            MRE engine = new MRE();
            var compiledRule = engine.CompileJSONRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsTrue(passes);

        }

        private string ReadfromResource(string FileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MicroRuleEngine.Tests.Files." + FileName;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
            //return string.Empty;
        }

        [TestMethod]
        public void JSONNestedPropertiesFalse()
        {
            string json = ReadfromResource("json2.json");
            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("config.rxstatus", mreOperator.Equal, "Pending") &
                Rule.Create("config.rxstatus", mreOperator.Equal, "Pending");
            MRE engine = new MRE();
            var compiledRule = engine.CompileJSONRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsTrue(passes);

        }
        [TestMethod]
        public void JsonrEqualvaluePassTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create( "PageNumber",  mreOperator.Equal, "90" ) &
            Rule.Create("RecordId", mreOperator.Equal, "788");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void JsonrEqualvalueFailTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("PageNumber", mreOperator.Equal, "91") &
            Rule.Create("RecordId", mreOperator.Equal, "788");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsFalse(passes);

        }

        [TestMethod]
        public void JsonrGraterFailTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("PageNumber", mreOperator.GreaterThan, "91") &
            Rule.Create("RecordId", mreOperator.Equal, "788");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsFalse(passes);

        }

        [TestMethod]
        public void JsonrGraterPassTest()
        {
            //Order order = ExampleUsage.GetOrder();
            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("PageNumber", mreOperator.GreaterThan, "89") &
            Rule.Create("RecordId", mreOperator.Equal, "788");

            MRE engine = new MRE();
            var compiledRule = engine.CompileXmlRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsTrue(passes);

        }

        [TestMethod]
        public void updateJSONElement()
        {
            string json =ReadfromResource("json1.json");
            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("Job.name", mreOperator.Equal, "UpdatedJob");
            MRE engine = new MRE();
            var compiledRule = Helpers.ChangePropertyValue<dynamic>(data, rule);
            //bool passes = compiledRule(data, "");


            //doc = MRE.ChangePropertyValue<XmlDocument>(doc, new Rule() { MemberName = "Status", TargetValue = "Cancelled" });
            Assert.IsTrue(Dynamitey.Dynamic.InvokeGetChain(compiledRule, "Job.name") == "UpdatedJob");
        }
        [TestMethod]
        public void JSONNestedProofing()
        {
            string json =ReadfromResource("json2.json");
            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("Job.ChildJob.name", mreOperator.Equal, "childjob1") &
                Rule.Create("Job.name", mreOperator.Equal, "Job1");
            MRE engine = new MRE();
            var compiledRule = engine.CompileJSONRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsFalse(passes);

        }

    }
}
