using System.Collections.Generic;
using System.Linq;

namespace Models.LogicRulesEngine
{
    public class AndRule<TContext> : IEvaluable<TContext>
    {
        private readonly List<IEvaluable<TContext>> _rules;

        public AndRule(IEnumerable<IEvaluable<TContext>> rules)
        {
            _rules = new List<IEvaluable<TContext>>(rules);
        }

        public bool Evaluate(TContext context)
        {
            return _rules.All(rule => rule.Evaluate(context));
        }
    }
}