using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using Models;
using Models.Log;
using Models.Rules;
using ViewModels.Messages;
using ViewModels.Rules;

namespace ViewModels.Modules
{
    public class Rules : Screen, IRules
    {
        private readonly ILogFilterEngine _filterEngine;

        public Rules(
            ILogFilterEngine filterEngine,
            RuleViewModelFactory ruleViewModelFactory
            )
        {
            _filterEngine = filterEngine;

            Root = (CompositeRuleViewModel)ruleViewModelFactory.Create(new CompositeRule() { Name = "Rules" }, null);
            TreeRoot = new BindableCollection<RuleViewModelBase>() { Root };

        }

        public BindableCollection<RuleViewModelBase> TreeRoot { get; }
        private RuleParentViewModelBase _root;
        public RuleParentViewModelBase Root
        {
            get { return _root; }
            set
            {
                if (Equals(value, _root)) return;
                _root = value;
                NotifyOfPropertyChange(() => Root);
            }
        }


        public Task<LogResult> Filter(AnalogLog logs)
        {
            var filter = Root.Rule.GetFilter();
            return  _filterEngine.FilterAsync(logs, filter);
        }

    }
}
