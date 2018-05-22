using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Models;
using Models.Log;
using ViewModels.Rules;

namespace ViewModels.Modules
{
    public interface IRules
    {
        BindableCollection<RuleViewModelBase> TreeRoot { get; }
        RuleParentViewModelBase Root { get; }
        Task<LogResult> Filter(AnalogLog logs);
    }
}