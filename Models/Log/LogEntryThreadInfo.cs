
namespace Models.Log
{
    public class LogEntryThreadInfo
    {
        public LogEntryThreadInfo(int threadId, string threadName)
        {
            ThreadId = threadId;
            ThreadName = threadName;
        }
        public int ThreadId { get; }
        public string ThreadName { get; }
    }
}
