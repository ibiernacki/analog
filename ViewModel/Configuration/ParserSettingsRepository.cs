using Models.Settings;
using System.Threading.Tasks;

namespace ViewModels.Configuration
{
    class ParserSettingsRepository : ISettingsRepository
    {
        private readonly IConfigurationManager _configurationManager;

        public ParserSettingsRepository(IConfigurationManager configurationManager)
        {
            _configurationManager = configurationManager;
        }

        public async Task<Settings> Read()
        {
            var configuration = await _configurationManager.Load();

            return Settings
                .NewBuilder()
                .ParserType(configuration.ParserType)
                .Build();
        }
    }
}
