using System;
using System.Configuration;
using System.Threading.Tasks;

namespace ViewModels.Configuration
{
    public class SettingsConfigurationProvider : IConfigurationProvider
    {
        private readonly Func<ConfigurationData> _configurationDataFactory;

        public SettingsConfigurationProvider(Func<ConfigurationData> configurationDataFactory)
        {
            _configurationDataFactory = configurationDataFactory;
        }

        public Task<ConfigurationData> Load()
        {
            return Task.Factory.StartNew(() =>
            {
                var configData = _configurationDataFactory();

                configData.NetworkRulesProviderUri = Properties.Settings.Default.Server;
                configData.SyntaxHighlighting = Properties.Settings.Default.SyntaxTransformer;
                configData.FavoriteLibraryRules = Properties.Settings.Default.FavoriteLibraryRules;
                configData.Fold = Properties.Settings.Default.Fold;

                return configData;
            });
        }

        public void Merge(ConfigurationData from, ConfigurationData to)
        {
            to.NetworkRulesProviderUri = from.NetworkRulesProviderUri;
            to.SyntaxHighlighting = from.SyntaxHighlighting;
            to.FavoriteLibraryRules = from.FavoriteLibraryRules;
            to.Fold = from.Fold;
        }

        public Task Commit(ConfigurationData configurationData)
        {
            return Task.Factory.StartNew(() =>
            {
                Properties.Settings.Default.Server = configurationData.NetworkRulesProviderUri;
                Properties.Settings.Default.SyntaxTransformer = configurationData.SyntaxHighlighting;
                Properties.Settings.Default.FavoriteLibraryRules = configurationData.FavoriteLibraryRules;
                Properties.Settings.Default.Fold = configurationData.Fold;
                Properties.Settings.Default.Save();
            });
        }
    }
}