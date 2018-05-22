using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Rules
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RegexRuleAction
    {
        [Description("Matches")]
        Matches,
        [Description("Not")]
        DoesNotMatch
    }
}