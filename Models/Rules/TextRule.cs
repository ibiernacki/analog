using System;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models.Rules
{
    public class TextRule : RuleBase
    {
        public TextRuleAction SelectedAction { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCaseSensitive { get; set; }
        public string Color { get; set; }

        public override IEvaluable<LogEntry> GetFilter()
        {
            if (!IsEnabled)
            {
                return LogicRule<LogEntry>.Pass;
            }

            switch (SelectedAction)
            {
                case TextRuleAction.Contains:
                    return GetContainsRule();
                case TextRuleAction.DoesNotContain:
                    return new NotRule<LogEntry>(GetContainsRule());
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private LogicRule<LogEntry> GetContainsRule()
        {
            var stringComparisonType = IsCaseSensitive
                ? StringComparison.InvariantCulture
                : StringComparison.InvariantCultureIgnoreCase;

            return new LogicRule<LogEntry>(entry => entry.FullContent.IndexOf(Text, stringComparisonType) != -1);
        }
    }
}