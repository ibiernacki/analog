using System;
using System.Threading.Tasks;

namespace ViewModels.Configuration
{
    public interface IConfigurationManager
    {
        Task<ConfigurationData> Load();
        Task Commit(Action<ConfigurationData> changesAction);

    }
}
