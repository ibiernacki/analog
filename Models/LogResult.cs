using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Models.Log;

namespace Models
{
    public class LogResult
    {

        public static LogResult Build(IList<LogEntry> entries)
        {
            var logTextBuilder = new StringBuilder();
            var lineNumber = 1u;
            var visualLogs = new List<VisualLogEntry>(entries.Count);
            var offset = 0L;
            foreach (var entry in entries)
            {
                var text = entry.ToString();
                var lines = text.Count(c => c == '\n');
                var visualLog = VisualLogEntry.FromLogEntry(entry, lineNumber, lines, offset, text.Length);
                lineNumber += (uint)lines;
                offset += (uint)text.Length;
                logTextBuilder.Append(text);
                visualLogs.Add(visualLog);

            }
            return new LogResult(visualLogs, logTextBuilder);
        }

        private LogResult(IList<VisualLogEntry> visualLogs, StringBuilder logTextStringBuilder)
        {
            Entries = new ReadOnlyCollection<VisualLogEntry>(visualLogs);
            LogText = logTextStringBuilder.ToString();
        }


        public ReadOnlyCollection<VisualLogEntry> Entries { get; }
        public string LogText { get; }

        public VisualLogEntry GetLogFromLine(int line)
        {
            var min = 0;
            var max = Entries.Count - 1;
            while (min <= max)
            {
                var mid = (min + max) / 2;
                var item = Entries[mid];
                var lineNumber = item.LineNumber;
                var lines = item.Lines;

                if (line >= lineNumber && line < lineNumber + lines)
                {
                    return item;
                }
                else if (line < Entries[mid].LineNumber)
                {
                    max = mid - 1;
                }
                else
                {
                    min = mid + 1;
                }

            }
            return null;
        }
    }
}