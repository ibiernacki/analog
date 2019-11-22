using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Models.Extensions;
using Models.Log;
using Models.LogParsing;

namespace Models
{

    public class AcwLogProvider : ILogProvider
    {
        private readonly AcwLogParser _logParser = new AcwLogParser(new AcwNewFormatLineParser());
        
        public async Task<AnalogLog> LoadAsync(IEnumerable<StreamInfo> streams)
        {
            var stopwatch = Stopwatch.StartNew();

            var logFiles = new ConcurrentBag<LogSource>();

            var logTasks = new List<Task<List<LogEntry>>>();
            foreach (var streamInfo in streams)
            {
               var readLogTask =  _logParser.ParseStream(streamInfo.StreamReader);
                logTasks.Add(readLogTask);
            }

            await Task.WhenAll(logTasks);

            var allEntries = await Task.WhenAll(logTasks);
            var orderedEntries = await Task.Factory.StartNew(() => allEntries.SelectMany(o => o).OrderBy(logEntry => logEntry.Time).ToArray());

            var elapsed = stopwatch.Elapsed;

            var entryArray = logFiles.ToArray();

            return new AnalogLog(entryArray, orderedEntries);
        }
    }
}