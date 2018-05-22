using System.Threading.Tasks;
using Models;
using Models.Log;

namespace ViewModels.Modules
{
    public interface ILogState
    {
        AnalogLog AllLogs { get; }
        LogResult FilteredLogs { get; }
        Task Load(AnalogLog log);
        Task Filter();
    }
}