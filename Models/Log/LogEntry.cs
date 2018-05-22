using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.Log
{
    public class LogEntry
    {
        private readonly LogSource _logFile;
        private readonly StringBuilder _contentStringBuilder = new StringBuilder();
        private string _contentBeforeTags;

        public LogEntry(
            LogSource logFile, 
            DateTime time, 
            LogLevel level, 
            IReadOnlyList<string> tags = null, 
            LogEntryThreadInfo threadInfo = null)
        {
            _logFile = logFile;
            Time = time;
            LogLevel = level;
            Tags = tags;
            ThreadInfo = threadInfo;
        }

        public LogSource LogFile => _logFile;
        public DateTime Time { get; }
        public LogLevel LogLevel { get; }
        public IEnumerable<string> Tags { get; }
        public LogEntryThreadInfo ThreadInfo { get; }
        public string ContentAfterTags => _contentStringBuilder.ToString();

        public string FullContent => _contentBeforeTags + _contentStringBuilder.ToString() + Environment.NewLine;

        public void AddContent(string contentBeforeTags, string contentAfterTags)
        {
            _contentBeforeTags = contentBeforeTags;
            _contentStringBuilder.Append(contentAfterTags);
        }

        public void AddLine(string line)
        {
            _contentStringBuilder.Append("\r\n");
            _contentStringBuilder.Append(line);
        }

        public override string ToString()
        {
            var logLevelPadded = $"[{LogLevel}]".PadRight(9);
            if (Tags != null && Tags.Any())
            {
                return $"{Time:yyyy-MM-dd HH:mm:ss.fff} {logLevelPadded} [{string.Join(",", Tags)}] {ContentAfterTags}{Environment.NewLine}";
            }

            return $"{Time:yyyy-MM-dd HH:mm:ss.fff} {logLevelPadded} {ContentAfterTags}{Environment.NewLine}";
        }
    }
}