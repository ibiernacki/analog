using System;
using System.Text;
using System.Threading.Tasks;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models.Rules
{
    public interface IRule
    {
        IEvaluable<LogEntry> GetFilter();
        string Name { get; set; }
        bool IsEnabled { get; set; }
    }
}
