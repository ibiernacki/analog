using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Models.Log;
using Models.LogicRulesEngine;

namespace Models.Extensions
{
    internal static class LogExtensions
    {

        internal static IEnumerable<LogEntry> Filter(this IEnumerable<LogEntry> entries, IEvaluable<LogEntry> filter)
        {
            return entries.Where(filter.Evaluate);
        }

        internal static Task<IEnumerable<LogEntry>> FilterAsync(this IEnumerable<LogEntry> entries,
            IEvaluable<LogEntry> filter) => Task.Run(() => entries.Filter(filter));
    }
}