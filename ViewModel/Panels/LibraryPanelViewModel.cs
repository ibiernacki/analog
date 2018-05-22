using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using Caliburn.Micro;
using Models.Rules;
using ViewModels.Services;
using System.Collections.Generic;
using ViewModels.Messages;
using System;
using MoreLinq;
using ViewModels.Configuration;
using ViewModels.Modules;
using ViewModels.Rules;

namespace ViewModels.Panels
{
    public class LibraryPanelViewModel : PanelBase
    {
        private readonly IEnumerable<IRulesProvider> _rulesProviders;
        private readonly IConfigurationManager _configurationManager;
        private readonly IRules _rules;
        private readonly Func<RuleInfo, RuleInfoViewModel> _ruleInfoVmFactory;
        private readonly BindableCollection<RuleInfoViewModel> _library;


        public LibraryPanelViewModel(
            IEnumerable<IRulesProvider> rulesProviders,
            IConfigurationManager configurationManager,
            IRules rules,
            Func<RuleInfo, RuleInfoViewModel> ruleInfoVmFactory)
            : base("Library")
        {
            _rulesProviders = rulesProviders;
            _configurationManager = configurationManager;
            _rules = rules;
            this._ruleInfoVmFactory = ruleInfoVmFactory;
            _library = new BindableCollection<RuleInfoViewModel>();
            Library = CollectionViewSource.GetDefaultView(_library);
            Library.SortDescriptions.Add(new SortDescription("Status", ListSortDirection.Descending));
            Library.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
            IsExpanded = true;
        }
        public void RevertIsFavorite(RuleInfoViewModel rule)
        {
            rule.IsFavorite = !rule.IsFavorite;
            Library.Refresh();
        }

        public void RemoveRule(RuleInfoViewModel rule)
        {
            _library.Remove(rule);
            Library.Refresh();
            _rulesProviders.ForEach(rp => rp.Remove(rule.RuleInfo.Id).GetAwaiter().GetResult());
        }


        private string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                if (string.IsNullOrWhiteSpace(value))
                {
                    Library.Filter = null;
                }
                else
                {

                    Library.Filter = item => ((RuleInfoViewModel)item).RuleInfo.Name.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) != -1;
                }

                NotifyOfPropertyChange();
            }
        }

        public async Task Reload()
        {

            var rules = (await Task.WhenAll(_rulesProviders.Select(rp => rp.Load())))
                .ToList();

            var favorites = (await _configurationManager.Load())
                            .FavoriteLibraryRules
                            ?.Cast<string>()
                            .ToList() ?? new List<string>();

            

            _library.Clear();
            _library.AddRange(rules.SelectMany(r => r).Select(ri => _ruleInfoVmFactory(ri)));
            
        }

        public void Apply(RuleInfoViewModel rule)
        {
            if (rule == null)
                return;
            _rules.Root.Add(rule.RuleInfo.Rule);
        }

        public ICollectionView Library { get; }
    }
}
