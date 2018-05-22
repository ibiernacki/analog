using System.Collections.Generic;
using System.Linq;
using Models;
using ViewModels.Configuration;

namespace ViewModels.Services
{
    public class DefaultFoldingService : IFoldingService
    {
        private readonly IConfigurationManager _configurationManager;

        public DefaultFoldingService(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public IEnumerable<Folding> Update(LogResult logResult)
        {
            var fold = _configurationManager.Load().Result.Fold;
            foreach (var entry in logResult.Entries)
            {
                if (entry.Lines <= 1) continue;

                var entryOffset = (int)entry.Offset;

                var logEntry = entry.LogEntry.ToString();


                var indexOfFirstLf = logEntry.IndexOf('\n');
                if (indexOfFirstLf == -1)
                {
                    continue;
                }

                var startOffset = entryOffset + indexOfFirstLf;
                if (logEntry[indexOfFirstLf - 1] == '\r')
                {
                    startOffset -= 1;
                }



                var endOffset = entryOffset + logEntry.Length;

                if (logEntry.EndsWith("\r\n"))
                {
                    endOffset -= 2;
                }
                else if (logEntry.EndsWith("\n"))
                {
                    endOffset -= 1;
                }

                if (startOffset >= endOffset)
                {
                    continue;
                }

                var folding = new Folding
                {
                    StartOffset = startOffset,
                    EndOffset = endOffset,
                    Name = "...",
                    IsFolded = fold
                };

                yield return folding;
            }
        }
    }
}