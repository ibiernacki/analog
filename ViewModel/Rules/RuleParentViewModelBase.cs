using System;
using System.Collections.Generic;
using Caliburn.Micro;
using Models.Rules;
using ViewModels.Messages;

namespace ViewModels.Rules
{
    public abstract class RuleParentViewModelBase : RuleViewModelBase
    {
        private readonly IEventAggregator _eventAggregator;

        public RuleParentViewModelBase(IRule rule, IEventAggregator eventAggregator)
            : base(rule)
        {
            _eventAggregator = eventAggregator;
        }

        public abstract void Remove(RuleViewModelBase rule, bool showOnSnackbar = true);
        public abstract void Add(IRule rule);
        public abstract IReadOnlyCollection<RuleViewModelBase> Rules { get; }

        public virtual void Load()
        {
            _eventAggregator.PublishOnCurrentThread(new LoadRuleMessage(this));
        }

        public virtual void Save(RuleViewModelBase rule)
        {
            _eventAggregator.PublishOnCurrentThread(new SaveRuleMessage(rule.Rule));
        }

        public abstract void Insert(int index, IRule rule);

        public abstract void Add(RuleViewModelBase ruleViewModel);
    }
}