using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Log;

namespace Models
{
    public interface ILogProvider
    {
        Task<AnalogLog> LoadAsync(IEnumerable<StreamInfo> streams);
    }
}
