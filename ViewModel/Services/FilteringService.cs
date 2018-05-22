using System;
using System.Threading.Tasks;
using ViewModels.Modules;

namespace ViewModels
{
    public class FilteringService : IFilteringService
    {
        public event EventHandler FilteringRequested;
        public Task ExecuteFilter() => Task.Run(() => FilteringRequested(this, EventArgs.Empty));
    }
}