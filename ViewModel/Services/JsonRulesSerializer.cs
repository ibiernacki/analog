using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Rules;
using Newtonsoft.Json;

namespace ViewModels.Services
{
    public class JsonRulesSerializer : IRulesSerializer
    {
        public IRule Deserialize(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return JsonConvert.DeserializeObject<IRule>(reader.ReadToEnd(), new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
            }
        }

        public void Serialize(IRule rule, Stream stream)
        {
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(JsonConvert.SerializeObject(rule, Formatting.Indented, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All }));
            }
        }
    }
}
