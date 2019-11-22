using LiteDB;
using Models.Settings;
using System;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    class SettingsRepository : ISettingsRepository
    {
        private readonly string _analogSettingsPath;
        private readonly LiteDbMapper _mapper;
        const string sinlgeSettingId = "singleSettingId";

        public SettingsRepository(LiteDbMapper mapper)
        {
            _analogSettingsPath = PathHelper.GetAnalogFilePath("analog.settings.db");
            _mapper = mapper;
        }

        public Task<Settings> Read()
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<SettingsDbModel>("settings");
                var settings = collection.FindById(sinlgeSettingId);
                return Task.FromResult(Settings
                    .NewBuilder()
                    .ParserType(settings != null ? settings.ParserType : ParserType.Acw)
                    .Build());
            }
        }

        public Task Save(Settings settings)
        {
            _mapper.EnsureRegistered();
            using (var db = CreateLiteDb())
            {
                var collection = db.GetCollection<SettingsDbModel>("settings");
                collection.Upsert(new SettingsDbModel
                {
                    Id = sinlgeSettingId,
                    ParserType = settings.ParserType
                });
                return Task.FromResult(false);
            }
        }

        private LiteDatabase CreateLiteDb()
        {
            return new LiteDatabase(GetLiteDbConnectionString());
        }

        private string GetLiteDbConnectionString()
        {
            return $"filename={_analogSettingsPath}; journal=false";
        }
    }
}
