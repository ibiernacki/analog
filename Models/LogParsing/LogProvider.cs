using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Models.Log;
using Models.LogParsing;
using Models.Settings;

namespace Models
{

    public class LogProvider : ILogProvider
    {
        private readonly ISettingsRepository _settingsRepository;

        public LogProvider(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<AnalogLog> LoadAsync(IEnumerable<StreamInfo> streams)
        {
            var stopwatch = Stopwatch.StartNew();

            var logFiles = new ConcurrentBag<LogSource>();

            var parser = await CreateParser();

            var logTasks = new List<Task<List<LogEntry>>>();
            foreach (var streamInfo in streams)
            {
               var readLogTask =  parser.ParseStream(streamInfo.StreamReader);
                logTasks.Add(readLogTask);
            }

            await Task.WhenAll(logTasks);

            var allEntries = await Task.WhenAll(logTasks);
            var orderedEntries = await Task.Factory.StartNew(() => allEntries.SelectMany(o => o).OrderBy(logEntry => logEntry.Time).ToArray());

            var elapsed = stopwatch.Elapsed;

            var entryArray = logFiles.ToArray();

            return new AnalogLog(entryArray, orderedEntries);
        }

        private async Task<ILogParser> CreateParser()
        {
            var settings = await _settingsRepository.Read();
            switch(settings.ParserType)
            {
                case ParserType.Acw: return new AcwLogParser(new AcwNewFormatLineParser());
                case ParserType.PipeDelimetered: return new LogParsing.PipeDelimeteredLogParser.LogParser();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}