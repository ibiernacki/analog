using System;
using System.Collections.Generic;
using System.Linq;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models.Rules
{
    public class CompositeRule : RuleBase
    {
        public CompositeRule()
        {
            Rules = new List<IRule>();
            SelectedType = RuleGroupType.And;
        }
        public RuleGroupType SelectedType { get; set; }

        public override IEvaluable<LogEntry> GetFilter()
        {
            if (!IsEnabled)
            {
                return LogicRule<LogEntry>.Pass;
            }

            switch (SelectedType)
            {
                case RuleGroupType.And:
                    return new AndRule<LogEntry>(Rules.Where(rule => rule.IsEnabled).Select(rule => rule.GetFilter()));
                case RuleGroupType.Or:
                    return new OrRule<LogEntry>(Rules.Where(rule => rule.IsEnabled).Select(rule => rule.GetFilter()));
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IList<IRule> Rules { get; set; }
    }
}