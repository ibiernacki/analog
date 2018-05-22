using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Rules
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum IdRuleAction
    {
        [Description("Before")]
        Before,
        [Description("After")]
        After
    }
}