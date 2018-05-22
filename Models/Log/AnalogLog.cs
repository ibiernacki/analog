namespace Models.Log
{
    public class AnalogLog
    {
        public LogSource[] Sources { get; }
        public LogEntry[] Entries { get; }
        public AnalogLog(LogSource[] sources, LogEntry[] entries)
        {
            Sources = sources;
            Entries = entries;
        }
    }
}