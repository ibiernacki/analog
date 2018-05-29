using System;
using System.Windows.Media;
using Caliburn.Micro;
using GongSolutions.Wpf.DragDrop;
using MaterialDesignColors;
using Models.Rules;
using ViewModels.Editors;

namespace ViewModels.Rules
{
    public abstract class RuleViewModelBase : PropertyChangedBase

    {
        public IRule Rule { get; }


        private RuleParentViewModelBase _parent;

        public RuleParentViewModelBase Parent
        {
            get { return _parent; }
            set
            {
                _parent = value;
                NotifyOfPropertyChange();
            }
        }

        public RuleViewModelBase(IRule rule)
        {
            Rule = rule;
        }

        public virtual string Name
        {
            get { return Rule.Name; }
            set
            {
                Rule.Name = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsEnabled
        {
            get { return Rule.IsEnabled; }
            set
            {
                Rule.IsEnabled = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                _isSelected = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _isFocused;
        public bool IsFocused
        {
            get => _isFocused;
            set
            {
                _isFocused = value;
                NotifyOfPropertyChange();
            }
        }

        public void Focus()
        {
            IsFocused = true;
        }

        public abstract void NegateRule();
        
        public virtual void RevertIsEnabled()
        => IsEnabled = !IsEnabled;

        public virtual void Select()
            => IsSelected = true;

    }
}