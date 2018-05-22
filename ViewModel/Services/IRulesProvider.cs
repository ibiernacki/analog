using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models.Rules;

namespace ViewModels.Services
{
    public interface IRulesProvider
    {
        string Name { get; }
        Task<IList<RuleInfo>> Load();
        Task<bool> Update(RuleInfo ruleInfo);
        Task<bool> Remove(Guid ruleId);
        Task<bool> Add(RuleInfo ruleInfo);
        Task<RuleInfo> FindById(Guid ruleId);
        Task<RuleInfo> FindByName(string name);

    }
}