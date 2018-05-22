using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models;
using Models.Log;

namespace ViewModels.Modules
{
    public interface ILogLoader
    {
        Task<AnalogLog> ShowLoadLogsDialog();
        Task<AnalogLog> LoadPaths(string[] paths);
    }
}
