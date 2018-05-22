using System;
using System.Linq;
using System.Text;
using Models.Extensions;
using Models.Rules;

namespace Models
{
    public class Log
    {
        public LogSource[] Sources { get; }
        public LogEntry[] Entries { get; }
        public Log(LogSource[] sources, LogEntry[] entries)
        {
            Sources = sources;
            Entries = entries;
        }
    }
}