using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Caliburn.Micro;
using ViewModels.Configuration;
using ViewModels.LineTransformers;
using ViewModels.Modules;

namespace ViewModels.StatusBar
{
    public class StatusBarViewModel : Screen
    {
        private readonly ILogVisualizer _visualizer;
        private readonly IConfigurationManager _configurationManager;
        private readonly ILogSyntax[] _logSyntaxes;

        public StatusBarViewModel(
            ILogVisualizer visualizer,
            IConfigurationManager configurationManager,
            ILogSyntax[] logSyntaxes)
        {
            _visualizer = visualizer;
            _configurationManager = configurationManager;
            _logSyntaxes = logSyntaxes;

            SelectedSyntax = _logSyntaxes.FirstOrDefault();
            SyntaxDefinitions =
                CollectionViewSource.GetDefaultView(_logSyntaxes);
        }

        private bool _isSyntaxPopupOpen;
        public bool IsSyntaxPopupOpen
        {
            get { return _isSyntaxPopupOpen; }
            set
            {
                _isSyntaxPopupOpen = value;
                NotifyOfPropertyChange();
            }
        }

        public void OpenSyntaxPopup()
        {
            IsSyntaxPopupOpen = true;
        }

        protected override async void OnActivate()
        {
            base.OnActivate();

            var config = await _configurationManager.Load();
            if (!string.IsNullOrEmpty(config.SyntaxHighlighting))
            {
                var definition =
                    SyntaxDefinitions.Cast<ILogSyntax>().FirstOrDefault(
                        s => string.Equals(s.Name, config.SyntaxHighlighting, StringComparison.InvariantCulture));

                if (definition != null)
                {
                    SelectedSyntax = definition;
                }
            }

        }

        public ICollectionView SyntaxDefinitions { get; }

        private ILogSyntax _selectedSyntax;
        public ILogSyntax SelectedSyntax
        {
            get
            {
                return _selectedSyntax;
            }
            set
            {
                if (_selectedSyntax == value)
                {
                    return;
                }

                if (_selectedSyntax != null)
                {
                    _visualizer.LineTransformers.Transformers.Remove(_selectedSyntax);
                }

                if (value != null)
                {
                    _visualizer.LineTransformers.Transformers.Add(value);
                }

                _selectedSyntax = value;
                _configurationManager.Commit(cd => cd.SyntaxHighlighting = value?.Name);
                NotifyOfPropertyChange(() => SelectedSyntax);
            }
        }
    }
}
