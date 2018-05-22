using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using Models;
using Models.Log;
using ViewModels.Messages;
using ViewModels.Services;

namespace ViewModels.Modules
{
    public class LogLoader : ILogLoader
    {
        private readonly IFileService _fileService;
        private readonly IDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogProvider _logProvider;
        private readonly Func<ProgressDialogViewModel> _progressDialogFactory;

        public LogLoader(IFileService fileService,
            IDialogService dialogService,
            IEventAggregator eventAggregator,
            ILogProvider logProvider,
            Func<ProgressDialogViewModel> progressDialogFactory
            )
        {
            _fileService = fileService;
            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _logProvider = logProvider;
            _progressDialogFactory = progressDialogFactory;
        }

        public async Task<AnalogLog> ShowLoadLogsDialog()
        {
            var files = _fileService.OpenMany();
            if (files.Length == 0)
            {
                return null;
            }

            return await LoadPaths(files);
        }

        public async Task<AnalogLog> LoadPaths(string[] paths)
        {
            var streams = _fileService.OpenMany(paths);

            var progressDialog = _progressDialogFactory();

            progressDialog.Message = $"Loading files...";

            var dialogTask = _dialogService.ShowDialogAsync(progressDialog);
            var log = await _logProvider.LoadAsync(streams);
            progressDialog.Close();

            await dialogTask;

            _eventAggregator.PublishOnCurrentThread(new SnackbarMessage() { Action = q => q.Enqueue($"{paths.Length} files loaded") });
            return log;
        }
    }
}