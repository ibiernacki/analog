using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Configuration
{
    public interface IConfigurationProvider
    {
        Task<ConfigurationData> Load();
        void Merge(ConfigurationData from, ConfigurationData to);
        Task Commit(ConfigurationData configurationData);
    }
}
