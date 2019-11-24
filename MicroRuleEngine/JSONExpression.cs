using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Text;

namespace MicroRuleEngine
{
    public class JSONExpression : IExpression
    {
        public Expression GetExpression(Expression prm, Rule rule)
        {
           return GetDynamicDataField(prm, rule);
        }
        private static Expression CustomDynamicMethodInvoker(Rule rule, Expression param, string MethodName)
        {
            return Expression.Call(
                    Expression.New(typeof(Helpers)),
                    typeof(Helpers).GetMethod(MethodName, new Type[] { typeof(object),
            typeof(string) }),
                    param,
                    Expression.Constant(rule.MemberName, typeof(string))
                    );
        }

        private static Expression GetDynamicDataField(Expression prm, Rule rule)
        {
            var type = typeof(string);
            Expression expMember;
            if (rule.Operator == mreOperator.GreaterThanOrEqual.ToString()
                || rule.Operator == mreOperator.GreaterThan.ToString()
                || rule.Operator == mreOperator.LessThan.ToString()
               || rule.Operator == mreOperator.LessThanOrEqual.ToString()
               )
            {
                expMember = CustomDynamicMethodInvoker(rule, prm, "GetJSONPropertyInt");
                type = typeof(int);
            }
            else
            {
                expMember = CustomDynamicMethodInvoker(rule, prm, "GetJSONPropertyValue"); ;

            }


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

        public override string ToString()
        {
            return typeof(object).ToString();
        }
    }
}
