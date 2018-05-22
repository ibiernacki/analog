using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Models.Rules;
using Newtonsoft.Json;

namespace ViewModels.Services
{
    public class LocalRulesProvider : IRulesProvider
    {
        public string Name { get; } = "Json File";
        public string Path { get; set; } = "rules.json";


        public async Task<IList<RuleInfo>> Load()
        {
            if (!File.Exists(Path))
            {
                return new List<RuleInfo>();
            }

            string json = null;
            using (var reader = new StreamReader(File.OpenRead(Path)))
            {
                json = await reader.ReadToEndAsync();
            }
            return JsonConvert.DeserializeObject<IList<RuleInfo>>(json, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
        }

        public async Task<bool> Update(RuleInfo ruleInfo)
        {
            var rules = await Load();
            var ruleToUpdate = rules.Select((rule, index) => new { Rule = rule, Index = index }).FirstOrDefault(ri => ri.Rule.Id == ruleInfo.Id);
            if (ruleToUpdate == null)
            {
                return false;
            }
            rules[ruleToUpdate.Index] = ruleInfo;

            Write(rules, Path);
            return true;
        }

        private void Write(IList<RuleInfo> rules, string path)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(rules, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
        }

        public async Task<bool> Remove(Guid ruleId)
        {
            var rules = await Load();
            var rule = rules.FirstOrDefault(r => r.Id == ruleId);
            if (rule == null)
            {
                return false;
            }
            rules.Remove(rule);
            Write(rules, Path);
            return true;
        }

        public async Task<bool> Add(RuleInfo ruleInfo)
        {
            var rules = await Load();
            rules.Add(ruleInfo);
            Write(rules, Path);
            return true;
        }

        public Task<RuleInfo> FindById(Guid ruleId)
        {
            throw new NotImplementedException();
        }

        public Task<RuleInfo> FindByName(string name)
        {
            throw new NotImplementedException();
        }
    }
}