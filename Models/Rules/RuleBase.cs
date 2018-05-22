using System;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models.Rules
{
    public abstract class RuleBase : IRule
    {
        public abstract IEvaluable<LogEntry> GetFilter();
        public virtual string Name { get; set; }
        public virtual bool IsEnabled { get; set; } = true;
    }
}