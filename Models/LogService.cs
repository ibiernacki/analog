using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models
{
    public class LogService : ILogService
    {
        private readonly ILogFilterEngine _logFilterEngine;
        private readonly ILogProvider _logProvider;

        public AnalogLog All { get; private set; }
        public LogResult Current { get; private set; }

        public LogService(ILogFilterEngine logFilterEngine, ILogProvider logProvider)
        {
            _logFilterEngine = logFilterEngine;
            _logProvider = logProvider;
        }

        public async Task Filter(IEvaluable<LogEntry> filter) => Current = await _logFilterEngine.FilterAsync(All, filter);

        public async Task Load(IEnumerable<StreamInfo> streams)
        {
            All = await _logProvider.LoadAsync(streams);
            Current = await Task.Factory.StartNew(() => LogResult.Build(All.Entries));
        }
    }
}