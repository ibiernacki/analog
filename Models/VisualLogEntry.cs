using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Log;

namespace Models
{
    public class VisualLogEntry
    {
        public LogEntry LogEntry { get; }
        public uint LineNumber { get; }
        public int Lines { get; }
        public long Offset { get; }
        public long Length { get; }

        private VisualLogEntry(LogEntry logEntry, uint lineNumber, int lines, long offset, long length)
        {
            LogEntry = logEntry;
            LineNumber = lineNumber;
            Lines = lines;
            Offset = offset;
            Length = length;
        }

        public static VisualLogEntry FromLogEntry(LogEntry logEntry, uint lineNumber, int lines, long offset, long length)
            => new VisualLogEntry(logEntry, lineNumber, lines, offset, length);
    }
}

