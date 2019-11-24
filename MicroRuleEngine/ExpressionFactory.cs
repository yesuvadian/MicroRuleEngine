using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MicroRuleEngine
{
   public class ExpressionFactory
    {
        static readonly Dictionary<string, IExpression> _displayRules = new Dictionary<string, IExpression>();

        public static IExpression GetExpressionObject(string type)
        {
            if (_displayRules.Count == 0)
                LoadAssemblies();
            IExpression mapEngineParser = null;
            if (_displayRules.TryGetValue(type, out mapEngineParser))
                return mapEngineParser;

            return null;
        }
        static void LoadAssemblies()
        {
            Assembly ass = Assembly.GetExecutingAssembly();
            var type = typeof(IExpression);
            foreach (TypeInfo ti in ass.DefinedTypes)
            {
                if (ti.ImplementedInterfaces.Contains(type))
                {
                    if (ti.FullName != null)
                    {
                        var obj = ass.CreateInstance(ti.FullName) as IExpression;
                        if (obj != null) _displayRules.Add(obj.ToString(), obj);
                    }
                }
            }
        }
    }
}
