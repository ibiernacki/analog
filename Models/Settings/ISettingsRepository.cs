
using System.Threading.Tasks;

namespace Models.Settings
{
    public interface ISettingsRepository
    {
        Task<Settings> Read();
    }
}
