using System;
using System.Threading.Tasks;

namespace ViewModels
{
    public interface IFilteringService
    {
        event EventHandler FilteringRequested;
        Task ExecuteFilter();
    }
}