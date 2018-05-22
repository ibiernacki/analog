using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Models;
using Models.Log;
using ViewModels.Messages;
using ViewModels.Services;

namespace ViewModels.Modules
{
    /// <summary>
    /// Store all logs which are currently stored in memory
    /// </summary>
    public class LogStateViewModel : Screen, ILogState
    {
        public ILogVisualizer LogVisualizer { get; }
        private readonly IRules _filtering;
        private readonly IFilteringService _filteringService;

        public LogStateViewModel(
            IRules filtering,
            ILogVisualizer logVisualizer,
            IFilteringService filteringService)
        {
            LogVisualizer = logVisualizer;

            _filtering = filtering;
            _filteringService = filteringService;
            _filteringService.FilteringRequested += FilteringServiceOnFilteringRequested;
        }

        private async void FilteringServiceOnFilteringRequested(object sender, EventArgs eventArgs) => await Filter();

        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);
            _filteringService.FilteringRequested -= FilteringServiceOnFilteringRequested;
        }

        public bool IsEmpty => AllLogs == null;

        private AnalogLog _allLogs;
        public AnalogLog AllLogs
        {
            get { return _allLogs; }
            set
            {
                _allLogs = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(IsEmpty));
            }
        }

        private LogResult _filteredLogs;
        public LogResult FilteredLogs
        {
            get => _filteredLogs;
            set
            {
                _filteredLogs = value;
                NotifyOfPropertyChange();
            }
        }

        public async Task Load(AnalogLog log)
        {
            AllLogs = log;
            await Filter();
        }

        public async Task Filter()
        {
            FilteredLogs = await _filtering.Filter(AllLogs);
            await LogVisualizer.Display(FilteredLogs);
        }
    }
}
