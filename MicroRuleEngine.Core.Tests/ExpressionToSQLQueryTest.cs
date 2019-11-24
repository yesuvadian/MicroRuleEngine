using EFModeling.Samples.DataSeeding;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MicroRuleEngine.Core.Tests
{
    [TestClass]
    public class ExpressionToSQLQueryTest
    {
        internal DbContextOptions<BloggingContext> GetDBOptions()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<BloggingContext>()
                   .UseSqlite(connection)
                   .Options;

            // Create the schema in the database
            using (var context = new BloggingContext(options))
            {
                context.Database.EnsureCreated();
            }
            return options;
        }
        [TestMethod]
        public void BasicExpressionEqualityExpression()
        {
            string xml = "<root><Country><CountryCode>USA</CountryCode></Country></root>";
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);


            ParameterExpression xmlParam = Expression.Parameter(typeof(XmlDocument));

            ParameterExpression stringParam = Expression.Parameter(typeof(string));

            Expression callExpr = Expression.Call(
        Expression.New(typeof(Helpers)),
        typeof(MicroRuleEngine.Helpers).GetMethod("GetNodeValue", new Type[] { typeof(XmlDocument),
            typeof(string) }),
        xmlParam,
        stringParam
        );

           
          var result=  Expression.Lambda<Func<XmlDocument,string,string>>(callExpr,xmlParam,stringParam).Compile();
           var final= result(doc, "Country.CountryCode");
        }

        [TestMethod]
        public void BasicEqualityExpression()
        {
           
            using (var context = new BloggingContext(GetDBOptions()))
            {
                context.Blogs.Add(new Blog { Url = "http://test.com" });
                context.SaveChanges();

                var testBlog = context.Blogs.FirstOrDefault(b => b.Url == "http://test.com");

                var fields = MRE.Member.GetFields(typeof(Blog));
                Rule rule = new Rule
                {
                    MemberName = "Url",
                    Operator = mreOperator.Equal.ToString("g"),
                    TargetValue = "http://test.com"
                };

                var blog2 = context.Blogs.Where(MRE.ToExpression<Blog>(rule, false)).FirstOrDefault();

                Assert.IsTrue(testBlog.BlogId == blog2.BlogId);

            }
        }
    }
}
