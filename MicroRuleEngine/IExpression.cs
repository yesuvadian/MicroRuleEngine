using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MicroRuleEngine
{
    public interface IExpression
    {
        Expression GetExpression(Expression prm, Rule rule);

    }
}
