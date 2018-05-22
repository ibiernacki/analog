using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ViewModels.Configuration
{
    public class ConfigurationManager : IConfigurationManager
    {
        private readonly Func<ConfigurationData> _configurationDataFactory;
        private readonly IConfigurationProvider[] _configurationProviders;

        public ConfigurationManager(IEnumerable<IConfigurationProvider> configurationProviders, Func<ConfigurationData> configurationDataFactory)
        {
            _configurationDataFactory = configurationDataFactory;
            _configurationProviders = configurationProviders.ToArray();
        }

        public async Task<ConfigurationData> Load()
        {
            var configurationData = _configurationDataFactory();

            var configs = _configurationProviders.Select(provider =>
                    new
                    {
                        Provider = provider,
                        ConfigurationTask = provider.Load()
                    })
                .ToList();

            await Task.WhenAll(configs.Select(c => c.ConfigurationTask));

            configs.ForEach(c => c.Provider.Merge(c.ConfigurationTask.Result, configurationData));
            return configurationData;
        }

        public async Task Commit(Action<ConfigurationData> changesAction)
        {
             await Load()
            .ContinueWith(t => { changesAction(t.Result);
                     return t.Result;
                 })
            .ContinueWith(t => _configurationProviders.ToList().ForEach(cp => cp.Commit(t.Result)));
        }
    }
}