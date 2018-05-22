using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using Models;
using Models.Rules;
using Models.Log;
using ViewModels.Services;
using System;
using MaterialDesignThemes.Wpf;
using ViewModels.Panels;

namespace ViewModels.Modules
{
    public class LogVisualizerViewModel : Screen, ILogVisualizer
    {
        private readonly IFoldingService _foldingService;
        private readonly IRules _rules;
        private readonly IFilteringService _filteringService;
        private readonly ISearchService _searchService;

        public LogVisualizerViewModel(IFoldingService foldingService,
            IRules rules, IFilteringService filteringService,
            IVisualTransformers visualTransformers,
            ISearchService searchService)
        {
            _foldingService = foldingService;
            _rules = rules;
            _filteringService = filteringService;
            _searchService = searchService;
            LineTransformers = visualTransformers;
            ContextMenuExtensions = new BindableCollection<IEditorContextMenuItem>() {
                new EditorContextMenuItem("Show logs before", ShowLogsBefore),
                new EditorContextMenuItem("Show logs after", ShowLogsAfter),
                new EditorContextMenuSeparator(),
                new EditorContextMenuItem("Filter by selection", FilterBySelection, PackIconKind.AlertOutline),
                new EditorContextMenuItem("Filter out selection", FilterOutSelection),
                new EditorContextMenuSeparator(),
                new EditorContextMenuItem("Add selection to Search panel", AddToSearch, PackIconKind.SearchWeb)

            };
        }

        private Task AddToSearch()
        {
            _searchService.AddCriterion(Selection.GetText());
            return Task.FromResult<object>(null);
        }

        private async Task FilterOutSelection()
        {
            _rules.Root.Add(new TextRule()
            {
                SelectedAction = TextRuleAction.DoesNotContain,
                Text = Selection.GetText(),
                Name = $"Not '{Selection.GetText()}'"
            });

            await _filteringService.ExecuteFilter();
        }

        private async Task FilterBySelection()
        {
            _rules.Root.Add(new TextRule()
            {
                SelectedAction = TextRuleAction.Contains,
                Text = Selection.GetText(),
                Name = $"Contains '{Selection.GetText()}'"
            });
            await _filteringService.ExecuteFilter();
        }

        private async Task ShowLogsAfter()
        {
            var logEntry = _currentResult.GetLogFromLine(Selection.StartPosition.Line).LogEntry;
            _rules.Root.Add(new DateRule() { Date = logEntry.Time, SelectedAction = DateRuleAction.After, Name = $"Logs after {logEntry.Time}" });
            await _filteringService.ExecuteFilter();

        }

        private async Task ShowLogsBefore()
        {
            var logEntry = _currentResult.GetLogFromLine(Selection.StartPosition.Line).LogEntry;
            _rules.Root.Add(new DateRule() { Date = logEntry.Time, SelectedAction = DateRuleAction.Before, Name = $"Logs before {logEntry.Time}" });
            await _filteringService.ExecuteFilter();
        }

        public TextDocument Document { get; } = new TextDocument();


        private LogResult _currentResult;

        private Selection _selection;
        public Selection Selection
        {
            get { return _selection; }
            set
            {
                _selection = value;
                NotifyOfPropertyChange();
            }
        }

        public async Task Display(LogResult result)
        {
            _currentResult = result;

            await Application.Current.Dispatcher.Invoke(async () =>
            {
                Document.Text = result.LogText;
                Document.UndoStack.ClearAll();
                Foldings = await Task.Factory.StartNew(() => _foldingService.Update(result).ToList());
            });

        }

        private IList<Folding> _foldings;

        public IList<Folding> Foldings
        {
            get { return _foldings; }
            set
            {
                _foldings = value;
                NotifyOfPropertyChange();
            }
        }

        public BindableCollection<IEditorContextMenuItem> ContextMenuExtensions { get; }
        public IVisualTransformers LineTransformers { get; }

    }
}
