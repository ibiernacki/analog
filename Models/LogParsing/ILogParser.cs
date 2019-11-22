using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Models.Log;

namespace Models.LogParsing
{
    public interface ILogParser
    {
        Task<List<LogEntry>> ParseStream(Stream stream);
    }
}