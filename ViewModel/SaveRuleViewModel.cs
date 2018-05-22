using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Models.Rules;
using ViewModels.Messages;
using ViewModels.Services;

namespace ViewModels
{
    public class SaveRuleViewModel : DialogBaseViewModel
    {
        private readonly SaveRuleMessage _saveRuleMessage;
        private readonly IEnumerable<IRulesProvider> _ruleProviders;

        public SaveRuleViewModel(SaveRuleMessage saveRuleMessage, IEnumerable<IRulesProvider> ruleProviders)
        {
            _saveRuleMessage = saveRuleMessage;
            _ruleProviders = ruleProviders;
            _name = saveRuleMessage.Rule.Name;
            RulesProviders = new BindableCollection<IRulesProvider>(ruleProviders);
            Groups = new BindableCollection<string>(new[] { "test" });
            SelectedRuleProvider = RulesProviders[0];
            SelectedGroup = Groups[0];
            Name = saveRuleMessage.Rule.Name;
        }

        public void Cancel(IInputElement inputElement)
        {
            Close();
        }

        public async Task Commit(IInputElement inputElement)
        {
            var existingRule = await SelectedRuleProvider.FindByName(Name);
            if (existingRule != null)
            {
                existingRule.Rule = _saveRuleMessage.Rule;
                existingRule.DateUpdated = DateTime.UtcNow;
                await SelectedRuleProvider.Update(existingRule);
                Close(inputElement);

                return;
            }

            _saveRuleMessage.Rule.Name = Name;

            var ruleInfo = new RuleInfo
            {
                Author = Environment.UserName,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Group = SelectedGroup,
                Id = Guid.NewGuid(),
                Rule = _saveRuleMessage.Rule,
                Name = _saveRuleMessage.Rule.Name
            };


            await SelectedRuleProvider.Add(ruleInfo);
            Close(inputElement);
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyOfPropertyChange();

            }
        }

        public BindableCollection<IRulesProvider> RulesProviders { get; }

        private IRulesProvider _selectedRuleProvider;

        public IRulesProvider SelectedRuleProvider
        {
            get
            {
                return _selectedRuleProvider;
            }
            set
            {
                _selectedRuleProvider = value;
                NotifyOfPropertyChange();
            }
        }

        public BindableCollection<string> Groups { get; }

        private string _selectedGroup;

        public string SelectedGroup
        {
            get
            {
                return _selectedGroup;
            }
            set
            {
                _selectedGroup = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
