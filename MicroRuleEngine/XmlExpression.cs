using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;
using System.Xml;

namespace MicroRuleEngine
{
    public class XmlExpression : IExpression
    {
        public Expression GetExpression(Expression prm, Rule rule)
        {
            return GetXmlDataField(prm, rule);

        }

        private static Expression GetXmlDataField(Expression prm, Rule rule)
        {
            var type = typeof(string);
            Expression expMember;
            if (rule.Operator == mreOperator.GreaterThanOrEqual.ToString()
                || rule.Operator == mreOperator.GreaterThan.ToString()
                || rule.Operator == mreOperator.LessThan.ToString()
               || rule.Operator == mreOperator.LessThanOrEqual.ToString()
               )
            {
                expMember = CustomXmlMethodInvoker(rule, prm, "GetNodeValueInt");
                type = typeof(int);
            }
            else
            {
                expMember = CustomXmlMethodInvoker(rule, prm, "GetNodeValue");

            }

            //var extract=  Expression.Call(prm,_miGetxmlItem.Value, typeof(string));
            //  Expression. _miGetxmlItem.Value

            Debug.Assert(type != null);

            if (type.IsClass || rule.MemberName.StartsWith("System.Nullable"))
            {
                //  equals "return  testValue == DBNull.Value  ? (typeName) null : (typeName) testValue"
                return Expression.Condition(Expression.Equal(expMember, Expression.Constant(null)),
                    Expression.Constant(null, type),
                    Expression.Convert(expMember, type));
            }
            else
                // equals "return (typeName) testValue"
                return Expression.Convert(expMember, type);
        }
        private static Expression CustomXmlMethodInvoker(Rule rule, Expression param, string MethodName)
        {
            return Expression.Call(
Expression.New(typeof(Helpers)),
typeof(MicroRuleEngine.Helpers).GetMethod(MethodName, new Type[] { typeof(XmlDocument),
            typeof(string) }),
param,
Expression.Constant(rule.MemberName, typeof(string))
);
        }
        public override string ToString()
        {
            return typeof(XmlDocument).ToString();
        }

    }
    
}
