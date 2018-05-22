using System;

namespace Models.LogicRulesEngine
{
    public class LogicRule<TContext> : IEvaluable<TContext>
    {
        private readonly Func<TContext, bool> _expression;

        public LogicRule(Func<TContext, bool> expression)
        {
            _expression = expression;
        }

        public bool Evaluate(TContext context)
        {
            return _expression(context);
        }

        public static LogicRule<TContext> Pass => new LogicRule<TContext>(T => true);
    }
}