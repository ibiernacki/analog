using System.Collections.Generic;
using System.Linq;

namespace Models.LogicRulesEngine
{
    public class OrRule<TContext> : IEvaluable<TContext>
    {
        private readonly List<IEvaluable<TContext>> _rules;

        public OrRule(IEnumerable<IEvaluable<TContext>> rules)
        {
            _rules = new List<IEvaluable<TContext>>(rules);
        }

        public bool Evaluate(TContext context)
        {
            return _rules.Any(rule => rule.Evaluate(context));
        }
    }
}