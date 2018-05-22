using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using Models.Rules;
using ViewModels.Messages;
using ViewModels.Rules;
using ViewModels.Services;

namespace ViewModels
{
    public class LoadRuleViewModel : DialogBaseViewModel
    {
        private readonly LoadRuleMessage _loadRuleMessage;
        private readonly IEnumerable<IRulesProvider> _ruleProviders;
        private readonly Func<RuleInfo, RuleInfoViewModel> _ruleInfoViewModelFactory;
        private readonly BindableCollection<RuleInfoViewModel> _rules;

        public LoadRuleViewModel(
            LoadRuleMessage loadRuleMessage,
            IEnumerable<IRulesProvider> ruleProviders,
            Func<RuleInfo, RuleInfoViewModel> ruleInfoViewModelFactory)
        {
            _loadRuleMessage = loadRuleMessage;
            _ruleProviders = ruleProviders;
            _ruleInfoViewModelFactory = ruleInfoViewModelFactory;

            _rules = new BindableCollection<RuleInfoViewModel>();
            Rules = new ListCollectionView(_rules);
            Rules.GroupDescriptions.Add(new PropertyGroupDescription("RuleInfo.Group"));

        }

        public async Task LoadAsync()
        {
            var tasks = _ruleProviders
                            .Select(rp => rp.Load()
                                .ContinueWith(t => _rules.AddRange(t.Result.Select(ri => _ruleInfoViewModelFactory(ri)))))
                            .ToArray();

            await Task.WhenAll(tasks);
        }

        public override void Close(IInputElement inputElement = null)
        {
            foreach (var ruleInfo in _rules.Where(r => r.IsSelected))
            {
                _loadRuleMessage.RuleParentViewModel.Add(ruleInfo.RuleInfo.Rule);
            }

            DialogHost.CloseDialogCommand.Execute(null, inputElement);
        }


        public ICollectionView Rules { get; }
    }
}