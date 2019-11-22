using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Models.Extensions;
using Models.Log;

namespace Models.LogParsing
{
    public class AcwLogParser : ILogParser
    {
        private readonly AcwNewFormatLineParser _newFormatLineParser;
        private const string Delimiter = " - ";

        public AcwLogParser(AcwNewFormatLineParser newFormatLineParser)
        {
            _newFormatLineParser = newFormatLineParser;
        }

        public async Task<List<LogEntry>> ParseStream(Stream stream)
        {
            return await Task.Run(async () =>
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
                            var entry = ParseLine(line);
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
            });
        }


        public LogEntry ParseLine(string line)
        {
            var logEntry = ParseCorrectLine(line);
            if (logEntry == null)
            {
                logEntry = ParseLineWithMissingLevelTag(line);
            }
            return logEntry;
        }
        

        private LogEntry ParseCorrectLine(string line)
        {
            var steveLine = _newFormatLineParser.ParseLine(line);
            if (steveLine != null)
            {
                return steveLine;
            }
           
            int firstDelimiter = line.IndexOf(Delimiter, StringComparison.InvariantCulture);
            if (firstDelimiter == -1)
            {
                return null;
            }
            string dateAndType = line.Substring(0, firstDelimiter);

            int indexOfType = dateAndType.IndexOf('[');

            if (indexOfType == -1)
            {
                return null;
            }
            int endIndexOfType = dateAndType.IndexOf(']');

            if (endIndexOfType == -1)
            {
                return null;
            }

            string date = dateAndType.Substring(0, indexOfType);

            DateTime entryTime;
            if (!DateTime.TryParse(date, out entryTime))
            {
                return null;
            }

            string typeString = dateAndType.Substring(indexOfType + 1, endIndexOfType - indexOfType - 1);


            if (!Enum.TryParse<LogLevel>(typeString, true, out var logLevel))
            {
                return null;
            }

            var indexToSplitContentAndTags = firstDelimiter + Delimiter.Length;

            string content = line.Substring(indexToSplitContentAndTags);

            var tags = ParseTags(content, out var lastTagIndex);


            LogEntry logFileEntry = new LogEntry(entryTime, logLevel, tags);

            if (lastTagIndex != -1)
            {
                var contentBeforeTags = line.Substring(0, indexToSplitContentAndTags + lastTagIndex);
                string contentWithoutTag = content.Substring(lastTagIndex);
                logFileEntry.AddContent(contentBeforeTags, contentWithoutTag);
            }
            else
            {
                logFileEntry.AddContent(string.Empty,string.Empty);
            }


            return logFileEntry;
        }

        private LogEntry ParseLineWithMissingLevelTag(string line)
        {
            string dateEndDelimiter = " ";

            if (line.Length < 15)
            {
                return null;
            }

            int firstDelimiter = line.IndexOf(dateEndDelimiter, 15, StringComparison.InvariantCulture);
            if (firstDelimiter == -1)
            {
                return null;
            }

            string date = line.Substring(0, firstDelimiter);

            if (!DateTime.TryParse(date, out var entryTime))
            {
                return null;
            }

            string content = line.Substring(firstDelimiter + dateEndDelimiter.Length);

            var tags = ParseTags(content, out var lastTagIndex);

            LogEntry logFileEntry = new LogEntry(entryTime, LogLevel.Unknown, tags);

            if (lastTagIndex != -1)
            {
                var contentBeforeTags = line.Substring(0, firstDelimiter + dateEndDelimiter.Length + lastTagIndex);

                string contentWithoutTag = content.Substring(lastTagIndex);

                logFileEntry.AddContent(contentBeforeTags, contentWithoutTag);
            }
            else
            {
                logFileEntry.AddContent(string.Empty,string.Empty);
            }


            return logFileEntry;
        }


        private IReadOnlyList<string> ParseTags(string content, out int tagIndex)
        {
            List<string> tags = new List<string>();

            int index = 0;
            for (; ; )
            {
                int tagOpenIndex = content.FirstNotOf(new[] { '-', '\t', ' ' }, index);

                if (tagOpenIndex == -1)
                {
                    tagIndex = index;
                    return tags;
                }

                if (content[tagOpenIndex] != '[')
                {
                    tagIndex = index;
                    return tags;
                }

                if (tagOpenIndex == -1)
                {
                    tagIndex = index;
                    return tags;
                }
                int lastTagIndex = content.IndexOf("]", tagOpenIndex + 1, StringComparison.Ordinal);
                if (lastTagIndex == -1)
                {
                    tagIndex = index;
                    return tags;
                }
                string tagString = content.Substring(tagOpenIndex + 1, lastTagIndex - tagOpenIndex - 1);
                if (tagString.Contains(" "))
                {
                    tagIndex = index;
                    return tags;
                }
                tags.AddRange(tagString.Split(','));

                index = content.FirstNotOf(new[] { ' ', '-', '\t' }, lastTagIndex + 1);
                if (index == -1)
                {
                    tagIndex = -1;
                    return tags;
                }
            }
        }
    }
}
