using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using LiteDB;
using Models.Rules;
using ViewModels.Helpers;

namespace ViewModels.Services
{
    public class LiteDbRulesProvider : IRulesProvider
    {
        private readonly LiteDbMapper _mapper;
        private const string RuleFileName = "analog.rules.db";
        private const string AnalogFolderName = "Analog";
        private readonly string _analogRulePath;


        public LiteDbRulesProvider(LiteDbMapper mapper)
        {
            _mapper = mapper;
            Name = "local (LiteDb)";
            _analogRulePath = GetAnalogRuleFilePath();
        }

        public string Name { get; }
        public Task<IList<RuleInfo>> Load()
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<RuleInfo>("rules");
                return Task.FromResult<IList<RuleInfo>>(collection.FindAll().ToList());
            }
        }

        public Task<bool> Update(RuleInfo ruleInfo)
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<RuleInfo>("rules");
                return Task.FromResult(collection.Update(ruleInfo));
            }
        }

        public Task<bool> Remove(Guid ruleId)
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<RuleInfo>("rules");
                return Task.FromResult(collection.Delete(ri => ri.Id == ruleId) > 0);

            }
        }

        public Task<RuleInfo> FindById(Guid ruleId)
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<RuleInfo>("rules");
                return Task.FromResult(collection.FindById(ruleId));
            }
        }

        public Task<RuleInfo> FindByName(string name)
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<RuleInfo>("rules");
                return Task.FromResult(collection.FindOne(r => r.Name == name));
            }
        }

        public Task<bool> Add(RuleInfo ruleInfo)
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<RuleInfo>("rules");
                collection.Insert(ruleInfo);
            }
            return Task.FromResult(true);

        }

        private LiteDatabase CreateLiteDb()
        {
            return new LiteDatabase(GetLiteDbConnectionString());
        }

        private string GetAnalogRuleFilePath()
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var analogFolderPath = Path.Combine(localAppData, AnalogFolderName);
            if (!Directory.Exists(analogFolderPath))
            {
                Directory.CreateDirectory(analogFolderPath);
            }
            return Path.Combine(analogFolderPath, RuleFileName);
        }

        private string GetLiteDbConnectionString()
        {
            return $"filename={_analogRulePath}; journal=false";
        }
    }
}