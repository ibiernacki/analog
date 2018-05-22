using System;

namespace Models.Rules
{
    public class RuleInfo
    {
        public Guid Id { get; set; }
        public string Author { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Group { get; set; }
        public IRule Rule { get; set; }
        public string Name { get; set; }
    }
}