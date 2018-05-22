using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Models.Rules
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum TextRuleAction
    {
        [Description("Contains")]
        Contains,
        [Description("Not")]
        DoesNotContain
    }
}