using System;
using System.Text.RegularExpressions;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models.Rules
{
    public class RegexRule : RuleBase
    {
        public RegexRuleAction SelectedAction { get; set; }
        public string Pattern { get; set; } = string.Empty;

        public string Color { get; set; }


        public override IEvaluable<LogEntry> GetFilter()
        {
            if (!IsEnabled)
            {
                return LogicRule<LogEntry>.Pass;
            }

            switch (SelectedAction)
            {
                case RegexRuleAction.Matches:
                    return GetMatchesRule();
                case RegexRuleAction.DoesNotMatch:
                    return new NotRule<LogEntry>(GetMatchesRule());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private LogicRule<LogEntry> GetMatchesRule()
        {
            return new LogicRule<LogEntry>(entry => new Regex(Pattern).IsMatch(entry.FullContent));
        }
    }
}