using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Caliburn.Micro;
using Models;
using Models.LogicRulesEngine;
using Models.Rules;
using ViewModels.Editors;
using ViewModels.Messages;
using ViewModels.Services;

namespace ViewModels.Rules
{
    public class CompositeRuleViewModel : RuleParentViewModelBase
    {
        private readonly CompositeRule _model;
        private readonly RuleViewModelFactory _rulesFactory;
        private readonly IEventAggregator _eventAggregator;
        public override IReadOnlyCollection<RuleViewModelBase> Rules { get; }
        private readonly BindableCollection<RuleViewModelBase> _rules;

        public CompositeRuleViewModel(CompositeRule rule, RuleViewModelFactory rulesFactory, IEventAggregator eventAggregator)
            : base(rule, eventAggregator)
        {
            _model = rule;
            _rulesFactory = rulesFactory;
            _eventAggregator = eventAggregator;

            _rules = new BindableCollection<RuleViewModelBase>(rule.Rules.Select(r => rulesFactory.Create(r, this)));
            Rules = _rules;
        }

        [Expose]
        public override string Name
        {
            get { return Rule.Name; }
            set
            {
                Rule.Name = value;
                NotifyOfPropertyChange();
            }
        }

        [Expose(OverridenType = typeof(Enum))]
        [DisplayName("Type")]
        [Description("Rule group type - it can be either \"And\" group or \"Or\" group. You can also swtich group type by double-clicking label near group name in tree view")]

        public RuleGroupType SelectedType
        {
            get { return _model.SelectedType; }
            set
            {
                _model.SelectedType = value;
                NotifyOfPropertyChange();
            }
        }

        public override void Remove(RuleViewModelBase rule, bool showOnSnackbar = true)
        {
            var index = _rules.IndexOf(rule);

            if (index == -1)
            {
                return;
            }

            _model.Rules.Remove(rule.Rule);
            _rules.Remove(rule);

            if (!showOnSnackbar)
            {
                return;
            }

            _eventAggregator.PublishOnCurrentThread(new SnackbarMessage()
            {
                Action = smq => smq.Enqueue($"Rule \"{rule.Name}\" removed", "UNDO",
                    () =>
                    {
                        _rules.Insert(index, rule);
                        _model.Rules.Add(rule.Rule);
                    }, true)
            });
        }

        public RuleViewModelBase AddTextRule()
        {
            var textRule = new TextRule() { Name = nameof(TextRule) };
            _model.Rules.Add(textRule);
            var vm = _rulesFactory.Create(textRule, this);
            _rules.Add(vm);
            vm.Focus();
            return vm;
        }

        public void AddDateRule()
        {
            var dateRule = new DateRule() { Name = nameof(DateRule) };
            _model.Rules.Add(dateRule);
            var ruleViewModel = _rulesFactory.Create(dateRule, this);
            _rules.Add(ruleViewModel);
            ruleViewModel.Focus();
        }

        public void AddRegexRule()
        {
            var regexRule = new RegexRule() { Name = nameof(RegexRule) };
            _model.Rules.Add(regexRule);
            var regexRuleViewModel = _rulesFactory.Create(regexRule, this);

            _rules.Add(regexRuleViewModel);
            regexRuleViewModel.Focus();
        }

        public void AddCompositeRule()
        {
            var compositeRule = new CompositeRule() { Name = nameof(CompositeRule) };
            _model.Rules.Add(compositeRule);
            _rules.Add(_rulesFactory.Create(compositeRule, this));
        }

        public override void Add(IRule rule)
        {
            _model.Rules.Add(rule);
            _rules.Add(_rulesFactory.Create(rule, this));
        }

        public override void Insert(int index, IRule rule)
        {
            _model.Rules.Insert(index, rule);
            _rules.Insert(index, _rulesFactory.Create(rule, this));
        }

        public override void Add(RuleViewModelBase ruleViewModel)
        {
            _rules.Add(ruleViewModel);
            _model.Rules.Add(ruleViewModel.Rule);
        }

        public void NegateRule()
        {
            SelectedType = SelectedType == RuleGroupType.And ? RuleGroupType.Or : RuleGroupType.And;
            _model.SelectedType = SelectedType;

        }
    }
}