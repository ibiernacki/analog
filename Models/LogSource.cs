using System;
using System.Collections.Generic;
using System.Linq;
using Models.Extensions;

namespace Models
{
    public class LogSource
    {
        public LogSource(string name)
        {
            Name = name;
        }
        public string Name { get; }
    }
}