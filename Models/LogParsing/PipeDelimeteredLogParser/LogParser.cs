using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Models.Log;

namespace Models.LogParsing.PipeDelimeteredLogParser
{
    public class LogParser : ILogParser
    {
        private static readonly Regex regex = new Regex(@"^(?<timestamp>\d{4}-\d\d-\d\d \d\d:\d\d:\d\d\.\d\d\d)\d?\|(?<level>INFO|DEBUG|ERROR|FATAL)\|(?<line>.*)", RegexOptions.Compiled);

        public async Task<List<LogEntry>> ParseStream(Stream stream)
        {
            LogEntry currentEntry = null;
            var logEntries = new List<LogEntry>();
            using (var bs = new BufferedStream(stream))
            {
                using (var reader = new StreamReader(bs, Encoding.Default))
                {
                    string line = null;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        var entry = ParseSingleLine(line);
                        if (entry != null)
                        {
                            logEntries.Add(entry);
                            currentEntry = entry;
                        }
                        else
                        {
                            currentEntry?.AddLine(line);
                        }
                    }
                }
            }
            return logEntries;
        }

        private LogEntry ParseSingleLine(string line)
        {
            var match = regex.Match(line);

            if (!match.Success) return null;

            var timestamp = DateTime.ParseExact(match.Groups["timestamp"].Value, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture);

            var stringLevel = match.Groups["level"].Value;
            LogLevel level = LogLevel.Unknown;

            switch(stringLevel)
            {
                case "DEBUG": level = LogLevel.Debug; break;
                case "ERROR": level = LogLevel.Error; break;
                case "INFO": level = LogLevel.Info; break;
                case "FATAL": level = LogLevel.Info; break;
            }

            var result = new LogEntry(timestamp, level);
            result.AddLine(match.Groups["line"].Value);
            return result;
        }
    }
}
