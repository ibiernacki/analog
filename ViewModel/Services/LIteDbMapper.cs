using LiteDB;
using Models.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public class LiteDbMapper
    {
        private bool _isRegistered;

        private void Initialize()
        {
            var mapper = BsonMapper.Global;

            mapper.Entity<RuleInfo>()
                .Id(x => x.Id);

            mapper.RegisterType<CompositeRule>(rule =>
            {
                var bson = new BsonDocument
                {
                    ["Type"] = new BsonValue(rule.GetType().Name),
                    [nameof(rule.SelectedType)] = new BsonValue(rule.SelectedType.ToString()),
                    [nameof(rule.IsEnabled)] = new BsonValue(rule.IsEnabled),
                    [nameof(rule.Name)] = new BsonValue(rule.Name)
                };
                bson.Add(nameof(rule.Rules), new BsonArray(rule.Rules.Select(r => mapper.ToDocument(r))));
                return bson;
            }, value =>
            {
                var rule = new CompositeRule();
                rule.IsEnabled = value.AsDocument[nameof(rule.IsEnabled)].AsBoolean;
                rule.Name = value.AsDocument[nameof(rule.Name)].AsString;
                rule.SelectedType =
                    (RuleGroupType)
                    Enum.Parse(typeof(RuleGroupType), value.AsDocument[nameof(rule.SelectedType)].AsString);
                rule.Rules = value.AsDocument[nameof(rule.Rules)].AsArray.Select(v =>
                {
                    var type =
                        typeof(RuleBase).Assembly.GetType(
                            $"{typeof(RuleBase).Namespace}.{v.AsDocument["Type"].AsString}", true);
                    return mapper.ToObject(type, v.AsDocument) as IRule;
                }).ToList();

                return rule;
            });
            mapper.RegisterType<TextRule>(
                serialize: rule =>
                {
                    var bson = new BsonDocument
                    {
                        ["Type"] = new BsonValue(rule.GetType().Name),
                        [nameof(rule.Name)] = new BsonValue(rule.Name),
                        [nameof(rule.IsEnabled)] = new BsonValue(rule.IsEnabled),
                        [nameof(rule.SelectedAction)] = new BsonValue(rule.SelectedAction.ToString()),
                        [nameof(rule.Text)] = new BsonValue(rule.Text),
                        [nameof(rule.IsCaseSensitive)] = new BsonValue(rule.IsCaseSensitive)
                    };
                    return bson;
                },
                deserialize: bson =>
                {
                    var rule = new TextRule();
                    rule.Name = bson.AsDocument[nameof(rule.Name)].AsString;
                    rule.IsEnabled = bson.AsDocument[nameof(rule.IsEnabled)].AsBoolean;
                    rule.IsCaseSensitive = bson.AsDocument[nameof(rule.IsCaseSensitive)].AsBoolean;
                    rule.SelectedAction =
                        (TextRuleAction)
                        Enum.Parse(typeof(TextRuleAction), bson.AsDocument[nameof(rule.SelectedAction)].AsString);
                    rule.Text = bson.AsDocument[nameof(rule.Text)].AsString;


                    return rule;
                });

            mapper.RegisterType<DateRule>(
                serialize: rule =>
                {
                    var bson = new BsonDocument
                    {
                        ["Type"] = new BsonValue(rule.GetType().Name),
                        [nameof(rule.Name)] = new BsonValue(rule.Name),
                        [nameof(rule.IsEnabled)] = new BsonValue(rule.IsEnabled),
                        [nameof(rule.SelectedAction)] = new BsonValue(rule.SelectedAction.ToString()),
                        [nameof(rule.Date)] = !rule.Date.HasValue ? BsonValue.Null : new BsonValue(rule.Date)
                    };
                    return bson;
                },
                deserialize: bson =>
                {
                    var rule = new DateRule();
                    rule.Name = bson.AsDocument[nameof(rule.Name)].AsString;
                    rule.IsEnabled = bson.AsDocument[nameof(rule.IsEnabled)].AsBoolean;
                    rule.SelectedAction = (DateRuleAction)Enum.Parse(typeof(DateRuleAction), bson.AsDocument[nameof(rule.SelectedAction)].AsString);
                    rule.Date = bson.AsDocument[nameof(rule.Date)].IsNull
                        ? new DateTime?()
                        : bson.AsDocument[nameof(rule.Date)].AsDateTime;

                    return rule;
                });

            mapper.RegisterType<RegexRule>(
                serialize: rule =>
                {
                    var bson = new BsonDocument
                    {
                        ["Type"] = new BsonValue(rule.GetType().Name),
                        [nameof(rule.Name)] = new BsonValue(rule.Name),
                        [nameof(rule.IsEnabled)] = new BsonValue(rule.IsEnabled),
                        [nameof(rule.SelectedAction)] = new BsonValue(rule.SelectedAction.ToString()),
                        [nameof(rule.Pattern)] = new BsonValue(rule.Pattern)
                    };
                    return bson;
                },
                deserialize: bson =>
                {
                    var rule = new RegexRule();
                    rule.Name = bson.AsDocument[nameof(rule.Name)].AsString;
                    rule.IsEnabled = bson.AsDocument[nameof(rule.IsEnabled)].AsBoolean;
                    rule.SelectedAction = (RegexRuleAction)
                       Enum.Parse(typeof(RegexRuleAction), bson.AsDocument[nameof(rule.SelectedAction)].AsString);
                    rule.Pattern = bson.AsDocument[nameof(rule.Pattern)].AsString;
                    return rule;
                });

            mapper.RegisterType<IRule>(
                serialize: rule => mapper.ToDocument(rule.GetType(), rule),
                deserialize: value =>
                {
                    var type =
                        typeof(RuleBase).Assembly.GetType(
                            $"{typeof(RuleBase).Namespace}.{value.AsDocument["Type"].AsString}", true);
                    return mapper.ToObject(type, value.AsDocument) as IRule;
                }
            );
        }

        public void EnsureRegistered()
        {
            if (_isRegistered)
                return;
            Initialize();
            _isRegistered = true;
        }
    }
}
