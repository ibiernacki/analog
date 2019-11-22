using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using Models.Extensions;
using Models.Log;

namespace Models.LogParsing
{
    public class AcwNewFormatLineParser
    {
        public LogEntry ParseLine(string line)
        {
            int currentIndex = 0;
            var logDateTime = ParseLogTime(line, currentIndex, ref currentIndex);
            if (logDateTime == null)
            {
                return null;
            }
            var logLevel = ParseLogLevel(line, currentIndex, ref currentIndex);
            if (logLevel == null)
            {
                return null;
            }
            var threadInfo = TryParseThreadInfo(line, currentIndex, ref currentIndex);
            var tags = ParseTags(line, currentIndex, ref currentIndex);


            var logEntry = new LogEntry(logDateTime.Value, logLevel.Value, tags, threadInfo);


            var firstNonSpaceIndex = line.FirstNotOf(new[] {' '}, currentIndex);
       

            var contentAfterMetadate = firstNonSpaceIndex == -1? string.Empty : line.Substring(firstNonSpaceIndex);
            var metadataContent = line.Substring(0, currentIndex);

            logEntry.AddContent(metadataContent, contentAfterMetadate);

            return logEntry;
        }

        private DateTime? ParseLogTime(string line, int currentIndex, ref int indexAfterParsing)
        {
            if (line.Length < 15 + currentIndex)
            {
                return null;
            }
            var spaceAfterTimeIndex = line.IndexOf(' ', 15 + currentIndex);
            if (spaceAfterTimeIndex == -1)
            {
                return null;
            }

            string dateTimeString = line.Substring(currentIndex, spaceAfterTimeIndex);

            if (!DateTime.TryParseExact(dateTimeString, "yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.AllowTrailingWhite, out DateTime dateTime))
            {
                return null;
            }
            indexAfterParsing = spaceAfterTimeIndex;
            return dateTime;
        }

        private LogLevel? ParseLogLevel(string line, int currentIndex, ref int indexAfterParsing)
        {
            var nextSpaceIndex = line.IndexOf(' ', currentIndex + 1);

            if (nextSpaceIndex == -1)
            {
                if (line.Length - currentIndex < 3)
                {
                    nextSpaceIndex = line.Length;
                }
                else
                {
                    return null;

                }
            }

            var logLevelString = line.Substring(currentIndex + 1, nextSpaceIndex - (currentIndex + 1));
            if (logLevelString.Length != 1)
            {
                return null;
            }

            var logLevel = ParseLogLevelCharacter(logLevelString[0]);
            indexAfterParsing = nextSpaceIndex;
            return logLevel;
        }


        private string ParseSquareBracketSection(string line, int currentIndex, ref int indexAfterParsing)
        {
            if (line.Length < currentIndex)
            {
                return null;
            }

            var openingBracketIndex = line.IndexOf('[', currentIndex);
            if (openingBracketIndex == -1)
            {
                return null;
            }

            var closingBracketIndex = line.IndexOf(']', currentIndex + 1);
            if (closingBracketIndex == -1)
            {
                return null;
            }

            if (openingBracketIndex > closingBracketIndex)
            {
                return null;
            }

            var contentBetweenBrackets = line.Substring(openingBracketIndex + 1, closingBracketIndex - openingBracketIndex - 1);
            indexAfterParsing = closingBracketIndex + 1;
            return contentBetweenBrackets;
        }

        private LogEntryThreadInfo TryParseThreadInfo(string line, int currentIndex, ref int indexAfterParsing)
        {
            int indexAfterSection = 0;
            var threadSectionContent = ParseSquareBracketSection(line, currentIndex, ref indexAfterSection);
            if (threadSectionContent == null)
            {
                return null;
            }
            var threadidAndNameString = threadSectionContent.Split('/');

            if (threadidAndNameString.Length == 2)
            {
                if (!int.TryParse(threadidAndNameString[0], out int threadId))
                {
                    return null;
                }
                var threadName = threadidAndNameString[1].Trim();
                indexAfterParsing = indexAfterSection;
                return new LogEntryThreadInfo(threadId, threadName);
            }
            if (threadidAndNameString.Length == 1)
            {
                if (!int.TryParse(threadidAndNameString[0], out int threadId))
                {
                    return null;
                }
                indexAfterParsing = indexAfterSection;

                return new LogEntryThreadInfo(threadId, string.Empty);
            }

            return null;
        }

        private IReadOnlyList<string> ParseTags(string line, int currentIndex, ref int indexAfterParsing)
        {
            int indexAfterSection = 0;
            var tagsContent = ParseSquareBracketSection(line, currentIndex, ref indexAfterSection);
            if (tagsContent == null)
            {
                return null;
            }
            var tags = tagsContent.Split(',');
            indexAfterParsing = indexAfterSection;
            return tags;
        }

        private LogLevel? ParseLogLevelCharacter(char logLevel)
        {
            switch (logLevel)
            {
                case 'I':
                    return LogLevel.Info;
                case 'W':
                    return LogLevel.Warning;
                case 'E':
                    return LogLevel.Error;
                case 'D':
                    return LogLevel.Debug;
            }
            return null;
        }


    }
}
