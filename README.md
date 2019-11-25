MicroRuleEngine is a single file rule engine
============================================

A `.Net` Rule Engine for **dynamically** evaluating business rules compiled on the fly.  If you have business rules that you don't want to hard code then the `MicroRuleEngine` is your friend.   The rule engine is easy to groc and is only about 200 lines of code.  Under the covers it creates a `Linq` expression tree that is compiled so even if your business rules get pretty large or you run them against thousands of items the performance should still compare nicely with a hard coded solution.

How To Install It?
------------------
Drop the code file into your app and change it as you wish.

How Do You Use It?
------------------
The best examples of how to use the `MicroRuleEngine (MRE)` can be found in the Test project included in the Solution.

One of the tests:
```csharp
	[TestMethod]
	public void ChildProperties()
	{
		Order order = this.GetOrder();
		Rule rule = new Rule()
		{
			MemberName = "Customer.Country.CountryCode",
			Operator = System.Linq.Expressions.ExpressionType.Equal.ToString("g"),
			TargetValue = "AUS"
		};
		MRE engine = new MRE();
		var compiledRule = engine.CompileRule<Order>(rule);
		bool passes = compiledRule(order);
		Assert.IsTrue(passes);

		order.Customer.Country.CountryCode = "USA";
		passes = compiledRule(order);
		Assert.IsFalse(passes);
	}
```

How to apply rule for XML?
------------------
The best examples of how to use the `MicroRuleEngine (MRE)` can be found in the Test project included in the Solution.

One of the tests:
```csharp
	 [TestMethod]
        public void XmlProperties()
        {
           
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
```

How to apply rule for JSON ?
------------------
The best examples of how to use the `MicroRuleEngine (MRE)` can be found in the Test project included in the Solution.

One of the tests:
```csharp
	 [TestMethod]
        public void JSONPropertiesTrue()
        {

            string json = "{  \"AnnotationId\": \"f573e938-7602-5f07-7d64-f92c162199ff\",\"RecordId\": \"788\",\"PageNumber\": \"90\",  \"ParentId\": \"fdc732ee-4e8d-c930-2d85-14933013998a\"}";

            dynamic data = Json.Decode(json);
            Rule rule = Rule.Create("RecordId", mreOperator.Equal, "788") &
                Rule.Create("PageNumber", mreOperator.Equal, "90");

            MRE engine = new MRE();
            var compiledRule = engine.CompileJSONRule<dynamic>(rule);
            bool passes = compiledRule(data, "");
            Assert.IsTrue(passes);

```

What Kinds of Rules can I express
--------------------------------
In addition to comparative operators such as `Equals`, `GreaterThan`, `LessThan` etc.   You can also call methods on the object that return a `boolean` value such as `Contains` or `StartsWith` on a string. In addition to comparative operators, additional operators such as `IsMatch` or `IsInteger` have been added and demonstrates how you could edit the code to add your own operator(s). Rules can also be `AND`'d or `OR`'d together:
```csharp

	Rule rule =
		Rule.Create("Customer.LastName", "Contains", "Do")
		& (
			Rule.Create("Customer.FirstName", "StartsWith", "Jo")
			| Rule.Create("Customer.FirstName", "StartsWith", "Bob")
		);
```

You can reference member properties which are `Arrays` or `List<>` by their index:
```csharp
	Rule rule = Rule.Create("Items[1].Cost", mreOperator.GreaterThanOrEqual, "5.25");
```

You can also compare an object to itself indicated by the `*.` at the beginning of the `TargetValue`:
```csharp
	Rule rule = Rule.Create("Items[1].Cost", mreOperator.Equal, "*.Items[0].Cost");
```

There are a lot of examples in the test cases but, here is another snippet demonstrating nested `OR` logic:
```csharp
	[TestMethod]
	public void ConditionalLogic()
	{
		Order order = this.GetOrder();
		Rule rule = new Rule()
		{
			Operator = "AndAlso",
			Rules = new List<Rule>()
			{
				new Rule() { MemberName = "Customer.LastName", TargetValue = "Doe", Operator = "Equal"},
				new Rule() { 
					Operator = "Or",
					Rules = new List<Rule>() {
						new Rule(){ MemberName = "Customer.FirstName", TargetValue = "John", Operator = "Equal"},
						new Rule(){ MemberName = "Customer.FirstName", TargetValue = "Judy", Operator = "Equal"}
					}
				}
			}
		};
		MRE engine = new MRE();
		var fakeName = engine.CompileRule<Order>(rule);
		bool passes = fakeName(order);
		Assert.IsTrue(passes);

		order.Customer.FirstName = "Philip";
		passes = fakeName(order);
		Assert.IsFalse(passes);
	}

```

If you need to run your comparison against an ADO.NET DataSet you can also do that as well:
```csharp
	Rule rule = Rule.Create("Items[1].Cost", mreOperator.Equal, "*.Items[0].Cost");
```

How Can I Store Rules?
---------------------
The `Rule` Class is just a **POCO** so you can store your rules as serialized `XML`, `JSON`, etc.

#### Forked many times and now updated to pull in a lot of the great work done by jamescurran, nazimkov and others that help improve the API
