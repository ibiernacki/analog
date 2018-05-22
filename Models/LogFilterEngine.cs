using System.Linq;
using System.Threading.Tasks;
using Models.Extensions;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models
{
    public class LogFilterEngine : ILogFilterEngine
    {
        public async Task<LogResult> FilterAsync(AnalogLog log, IEvaluable<LogEntry> filter)
        {
            var logs = log != null
                ? await log.Entries.FilterAsync(filter)
                : Enumerable.Empty<LogEntry>();

            return await Task.Factory.StartNew(() => LogResult.Build(logs.ToList()));
        }
    }
}