using System;
using System.ComponentModel;
using System.Windows.Media;
using MaterialDesignColors;
using Models.Rules;
using ViewModels.Editors;
using ViewModels.Helpers;

namespace ViewModels.Rules
{
    public class TextRuleViewModel : RuleViewModelBase
    {
        private readonly TextRule _rule;

        public TextRuleViewModel(TextRule rule)
            : base(rule)
        {
            _rule = rule;
            RefreshName();
        }

        [Expose(OverridenType = typeof(Enum))]
        [DisplayName("Action")]
        public TextRuleAction SelectedAction
        {
            get { return _rule.SelectedAction; }
            set
            {
                _rule.SelectedAction = value;
                NotifyOfPropertyChange();
                RefreshName();
            }
        }

        [Expose]
        [DisplayName("Case sensitive")]
        public bool IsCaseSensitive
        {
            get { return _rule.IsCaseSensitive; }
            set
            {
                if (value == _rule.IsCaseSensitive) return;
                _rule.IsCaseSensitive = value;
                NotifyOfPropertyChange();
            }

        }

        [Expose]
        public string Text
        {
            get { return _rule.Text; }
            set
            {
                if (value == _rule.Text) return;
                _rule.Text = value;
                NotifyOfPropertyChange();
                RefreshName();
            }
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


        public override void NegateRule()
        {
            SelectedAction = SelectedAction == TextRuleAction.Contains
                ? TextRuleAction.DoesNotContain
                : TextRuleAction.Contains;
            _rule.SelectedAction = SelectedAction;
        }
        


        private void RefreshName()
        {
            Name = $"{SelectedAction.GetDescription()}";
        }
    }


}