using System;
using System.ComponentModel;
using Models.Rules;
using ViewModels.Editors;
using ViewModels.Helpers;

namespace ViewModels.Rules
{
    public class DateRuleViewModel : RuleViewModelBase
    {
        private readonly DateRule _rule;

        public DateRuleViewModel(DateRule rule)
            : base(rule)
        {
            _rule = rule;
            RefreshName();
        }

        [Expose(OverridenType = typeof(Enum))]
        [DisplayName("Action")]
        public DateRuleAction SelectedAction
        {
            get { return _rule.SelectedAction; }
            set
            {
                if (value == _rule.SelectedAction) return;
                _rule.SelectedAction = value;
                NotifyOfPropertyChange(() => SelectedAction);
                RefreshName();
            }
        }

        [Expose(OverridenType = typeof(DateTime))]
        public DateTime? Date
        {
            get { return _rule.Date; }
            set
            {
                if (value.Equals(_rule.Date)) return;
                _rule.Date = value;
                NotifyOfPropertyChange(() => Date);
                RefreshName();
            }
        }

        private void RefreshName()
        {
            Name = $"{SelectedAction.GetDescription()} {Date}";
        }

        public override void NegateRule()
        {
            SelectedAction = SelectedAction == DateRuleAction.After ? DateRuleAction.Before : DateRuleAction.After;
        }
    }
}