using System;
using Models.Rules;
using System.Collections;
using System.Collections.Generic;
using Caliburn.Micro;
using ViewModels.Services;

namespace ViewModels.Rules
{
    public class RuleViewModelFactory
    {
        private readonly Func<IEventAggregator> _eventAggregatorFactory;

        public RuleViewModelFactory(Func<IEventAggregator> eventAggregatorFactory)
        {
            _eventAggregatorFactory = eventAggregatorFactory;
            _typeActivators = new Dictionary<Type, Func<IRule, RuleParentViewModelBase, RuleViewModelBase>>
            {
                [typeof(CompositeRule)] = (IRule rule, RuleParentViewModelBase vm) => new CompositeRuleViewModel((CompositeRule)rule, this, _eventAggregatorFactory()) { Parent = vm },
                [typeof(TextRule)] = (IRule rule, RuleParentViewModelBase vm) => new TextRuleViewModel((TextRule)rule) { Parent = vm },
                [typeof(DateRule)] = (IRule rule, RuleParentViewModelBase vm) => new DateRuleViewModel((DateRule)rule) { Parent = vm },
                [typeof(RegexRule)] = (IRule rule, RuleParentViewModelBase vm) => new RegexRuleViewModel((RegexRule)rule) { Parent = vm },
            };
        }

        private readonly IDictionary<Type, Func<IRule, RuleParentViewModelBase, RuleViewModelBase>> _typeActivators;

        public RuleViewModelBase Create(IRule rule, RuleParentViewModelBase parent)
        {
            return _typeActivators[rule.GetType()](rule, parent);
        }
    }
}