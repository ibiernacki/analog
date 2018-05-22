namespace Models.LogicRulesEngine
{
    public class NotRule<TContext> : IEvaluable<TContext>
    {
        private readonly IEvaluable<TContext> _rule;

        public NotRule(IEvaluable<TContext> rule)
        {
            _rule = rule;
        }

        public bool Evaluate(TContext context)
        {
            return !_rule.Evaluate(context);
        }
    }
}