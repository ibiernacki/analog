using Models.Rules;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public interface IRulesSerializer
    {
        void Serialize(IRule rule, Stream stream);
        IRule Deserialize(Stream stream);
    }
}
