using ViewModels.Rules;

namespace ViewModels.Messages
{
    public class LoadRuleMessage
    {
        public RuleParentViewModelBase RuleParentViewModel { get; }

        public LoadRuleMessage(RuleParentViewModelBase ruleParentViewModel)
        {
            RuleParentViewModel = ruleParentViewModel;
        }
    }
}