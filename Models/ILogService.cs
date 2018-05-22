using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models
{
    public interface ILogService
    {
        AnalogLog All { get; }
        LogResult Current { get; }
        Task Filter(IEvaluable<LogEntry> filter);
        Task Load(IEnumerable<StreamInfo> streams);
    }
}