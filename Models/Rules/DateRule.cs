using System;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models.Rules
{
    public class DateRule : RuleBase
    {
        public DateRuleAction SelectedAction { get; set; }
        public DateTime? Date { get; set; }
        public override IEvaluable<LogEntry> GetFilter()
        {
            if (!IsEnabled)
            {
                return LogicRule<LogEntry>.Pass;
            }

            switch (SelectedAction)
            {
                case DateRuleAction.Before:
                    return GetBeforeRule();
                case DateRuleAction.After:
                    return GetAfterRule();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private LogicRule<LogEntry> GetBeforeRule()
        {
            return new LogicRule<LogEntry>(entry => entry.Time <= Date);
        }

        private LogicRule<LogEntry> GetAfterRule()
        {
            return new LogicRule<LogEntry>(entry => entry.Time >= Date);
        }
    }
}