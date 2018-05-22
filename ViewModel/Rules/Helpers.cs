using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Rules
{
    internal static class Helpers
    {
        public static bool IsParentOf(this RuleParentViewModelBase ruleParent, RuleViewModelBase rule)
        {
            return ruleParent.TraverseRulesTree()
                .Any(r => r == rule);
        }

        private static IEnumerable<RuleViewModelBase> TraverseRulesTree(this RuleParentViewModelBase parent)
        {
            foreach (var rule in parent.Rules)
            {
                yield return rule;

                var ruleParent = rule as RuleParentViewModelBase;
                if (ruleParent == null)
                {
                    continue;
                }

                foreach (var childRule in ruleParent.TraverseRulesTree())
                {
                    yield return childRule;
                }
            }
        }
    }
}
