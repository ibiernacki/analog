using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;
using ViewModels.Messages;
using ViewModels.Services;
using SnackbarMessage = ViewModels.Messages.SnackbarMessage;
using GongSolutions.Wpf.DragDrop;
using ViewModels.DropTargets;
using ViewModels.Modules;
using ViewModels.Panels;
using ViewModels.StatusBar;
using Models.Settings;
using ViewModels.Configuration;

namespace ViewModels
{
    public class ShellViewModel :
        Conductor<Screen>.Collection.AllActive,
        IHandle<SnackbarMessage>,
        IHandle<LoadRuleMessage>,
        IHandle<SaveRuleMessage>,
        IDropTarget
    {
        private readonly IFileService _fileService;
        private readonly ILogLoader _logLoader;
        private readonly IEventAggregator _eventAggregator;
        private readonly SnackbarService _snackbarService;
        private readonly IDialogService _dialogService;
        private readonly IConfigurationManager _configurationManager;
        private readonly Func<SaveRuleMessage, SaveRuleViewModel> _saveRuleFactory;
        private readonly Func<LoadRuleMessage, LoadRuleViewModel> _loadRuleFactory;
        private readonly DropService _dropService;

        public PanelsViewModel Plugins { get; }
        public SnackbarService Snackbar { get; }
        public LibraryPanelViewModel LibraryPanel { get; }

        public StatusBarViewModel StatusBar { get; }

        public ShellViewModel(
            IFileService fileService,
            ILogLoader logLoader,
            PanelsViewModel plugins,
            IEventAggregator eventAggregator,
            SnackbarService snackbarService,
            IDialogService dialogService,
            IConfigurationManager configurationManager,
            Func<SaveRuleMessage, SaveRuleViewModel> saveRuleFactory,
            Func<LoadRuleMessage, LoadRuleViewModel> loadRuleFactory,
            LibraryPanelViewModel libraryPanel,
            Func<LogStateViewModel> logStateFactory,
            DropService dropService,
            StatusBarViewModel statusBar)
        {
            _fileService = fileService;
            _logLoader = logLoader;

            _eventAggregator = eventAggregator;
            _snackbarService = snackbarService;
            _dialogService = dialogService;
            _configurationManager = configurationManager;
            _saveRuleFactory = saveRuleFactory;
            _loadRuleFactory = loadRuleFactory;
            _dropService = dropService;

            Plugins = plugins;

            Snackbar = snackbarService;
            LibraryPanel = libraryPanel;
            StatusBar = statusBar;
            _eventAggregator.Subscribe(this);

            LogState = logStateFactory();
            Items.AddRange(new Screen[] { plugins, statusBar});
            DisplayName = "ANALOG";
        }

        public LogStateViewModel LogState { get; }

        public async Task Filter()
        {
            await Plugins.Filter();
        }

        public async Task OpenManyFiles()
        {
            var files = _fileService.OpenMany();
            if (files.Length == 0)
            {
                return;
            }

            await LoadFiles(files);
        }

        public Task UseAcwParser()
        {
            return UseParser(ParserType.Acw);
        }

        public Task UsePipeParser()
        {
            return UseParser(ParserType.PipeDelimetered);
        }

        private async Task UseParser(ParserType parserType)
        {
            await _configurationManager.Commit(cd =>
            {
                cd.ParserType = parserType;
            });
            DisplayParserType(parserType);
        }

        private void DisplayParserType(ParserType parserType)
        {
            UsesAcwParser = parserType == ParserType.Acw;
            UsesPipeParser = parserType == ParserType.PipeDelimetered;
        }

        public async Task Loaded()
        {
            var config = await _configurationManager.Load();
            DisplayParserType(config.ParserType);
            await LibraryPanel.Reload();
        }

        public void Minimize(Window window) => window.WindowState = WindowState.Minimized;
        public void Maximize(Window window) =>
            window.WindowState = window.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        public void Handle(SnackbarMessage message)
        {
            message.Action(_snackbarService.Queue);
        }

        public async void Handle(LoadRuleMessage message)
        {
            var loadRuleVm = _loadRuleFactory(message);
            var loadTask = loadRuleVm.LoadAsync();
            await _dialogService.ShowDialogAsync(loadRuleVm).ContinueWith(t =>
            {
                if (!loadTask.IsCompleted)
                {
                    //cancel
                }
            });

        }

        public async void Handle(SaveRuleMessage message)
        {
            var saveRuleVm = _saveRuleFactory(message);
            await _dialogService.ShowDialogAsync(saveRuleVm);
            await LibraryPanel.Reload();
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (_dropService.CanHandle(dropInfo))
            {
                dropInfo.Effects = DragDropEffects.Copy;
            }
        }

        public async void Drop(IDropInfo dropInfo)
        {
            await _dropService.Handle(dropInfo);
        }

        private async Task LoadFiles(string[] paths)
        {
           await LogState.Load(await _logLoader.LoadPaths(paths));
        }

        private string _logStatistics;
        public string LogStatistics
        {
            get => _logStatistics;
            set
            {
                _logStatistics = value;
                NotifyOfPropertyChange();
            }
        }
        private bool _usesAcwParser;

        public bool UsesAcwParser
        {
            get => _usesAcwParser;
            set
            {
                _usesAcwParser = value;
                NotifyOfPropertyChange();
            }
        }

        private bool _usesPipeParser;

        public bool UsesPipeParser
        {
            get => _usesPipeParser; 
            set
            {
                _usesPipeParser = value;
                NotifyOfPropertyChange();
            }
        }
    }
}