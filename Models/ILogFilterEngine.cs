using System.Threading.Tasks;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models
{
    public interface ILogFilterEngine
    {
        Task<LogResult> FilterAsync(AnalogLog log, IEvaluable<LogEntry> filter);
    }
}