using Models.Rules;

namespace ViewModels.Messages
{
    public class SaveRuleMessage
    {
        public IRule Rule { get; }

        public SaveRuleMessage(IRule rule)
        {
            Rule = rule;
        }
    }
}