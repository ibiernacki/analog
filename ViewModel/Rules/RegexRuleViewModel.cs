using System;
using System.Windows.Media;
using MaterialDesignColors;
using Models.Rules;
using ViewModels.Editors;
using ViewModels.Helpers;

namespace ViewModels.Rules
{
    public class RegexRuleViewModel : RuleViewModelBase
    {
        private readonly RegexRule _rule;

        public RegexRuleViewModel(RegexRule rule)
            : base(rule)
        {
            _rule = rule;
            RefreshName();
        }

        [Expose(OverridenType = typeof(Enum))]
        public RegexRuleAction SelectedAction
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

        [Expose]
        public string Pattern
        {
            get { return _rule.Pattern; }
            set
            {
                if (value == _rule.Pattern) return;
                _rule.Pattern = value;
                NotifyOfPropertyChange(() => Pattern);
                RefreshName();
            }
        }

        public override void NegateRule()
        {
            SelectedAction = SelectedAction == RegexRuleAction.Matches
                ? RegexRuleAction.DoesNotMatch
                : RegexRuleAction.Matches;
            _rule.SelectedAction = SelectedAction;

        }

        public virtual void SetColor(Hue color)
        {
            Color = color?.Color;
        }

        public Color? Color
        {
            get
            {
                return string.IsNullOrEmpty(_rule.Color)
                    ? new Color?()
                    : (Color)ColorConverter.ConvertFromString(_rule.Color);
            }
            set
            {
                _rule.Color = value.HasValue ? value.ToString() : null as string;
                NotifyOfPropertyChange();
            }
        }

        private void RefreshName()
        {
            Name = $"{SelectedAction.GetDescription()}";
        }
    }
}