using Autofac;
using ICSharpCode.AvalonEdit.Rendering;
using MaterialDesignThemes.Wpf;
using Models;
using ViewModels.Configuration;
using ViewModels.DropTargets;
using ViewModels.Editors;
using ViewModels.LineTransformers;
using ViewModels.Modules;
using ViewModels.Panels;
using ViewModels.Rules;
using ViewModels.Services;
using ViewModels.StatusBar;

namespace ViewModels
{
    public class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShellViewModel>()
                .SingleInstance();
            builder.RegisterType<PanelsViewModel>()
                .SingleInstance();
            builder.RegisterType<FileService>()
                .As<IFileService>()
                .SingleInstance();
            builder.RegisterType<PlainTextTransformer>()
                .As<ILogSyntax>();
            builder.RegisterType<AcwLineTransformer>()
                .As<ILogSyntax>();
            builder.RegisterType<SearchTransformer>()
                .As<ILineTransformer>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<EditorFactory>()
                .As<IEditorFactory>()
                .SingleInstance();

            builder.RegisterType<RuleViewModelFactory>();
            builder.RegisterType<JsonRulesSerializer>()
                .As<IRulesSerializer>();
            builder.RegisterType<LogProvider>()
                .As<ILogProvider>();
            builder.RegisterType<LogFilterEngine>()
                .As<ILogFilterEngine>();

            builder.RegisterType<LiteDbRulesProvider>()
                .As<IRulesProvider>();
            builder.RegisterType<LocalRulesProvider>()
                .As<IRulesProvider>();

            builder.RegisterType<ConfigurationManager>()
                .As<IConfigurationManager>()
                .SingleInstance();

            builder.RegisterType<SettingsConfigurationProvider>()
                .As<IConfigurationProvider>();

            builder.RegisterType<LogService>()
                .As<ILogService>();


            builder.RegisterType<PropertiesPanelViewModel>()
                .AsSelf()
                .As<IPanel>()
                .SingleInstance();

            builder.RegisterType<RulesPanelViewModel>()
                .AsSelf()
                .As<IPanel>()
                .SingleInstance();

            builder.RegisterType<SearchPanelViewModel>()
                .As<IPanel>()
                .As<ISearchService>()
                .SingleInstance();


            builder.RegisterType<LibraryPanelViewModel>()
                .As<IPanel>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<ConfigurationData>();
            builder.RegisterType<SnackbarMessageQueue>();
            builder.RegisterType<LiteDbMapper>()
                .SingleInstance();

            builder.RegisterType<SaveRuleViewModel>();
            builder.RegisterType<LoadRuleViewModel>();
            builder.RegisterType<RuleInfoViewModel>();
            builder.RegisterType<SnackbarService>();
            builder.RegisterType<ProgressDialogViewModel>();
            builder.RegisterType<DialogService>().As<IDialogService>();

            builder.RegisterType<Modules.Rules>()
                .As<IRules>()
                .SingleInstance();

            builder.RegisterType<LogStateViewModel>()
                .As<ILogState>()
                .As<LogStateViewModel>()
                .SingleInstance();

            builder.RegisterType<LogSyntaxProvider>()
                .As<ILogSyntaxProvider>();

            builder.RegisterType<LogVisualizerViewModel>()
                .As<ILogVisualizer>()
                .SingleInstance();

            builder.RegisterType<FilteringService>()
                .As<IFilteringService>()
                .SingleInstance();

            builder.RegisterType<LogLoader>()
                .As<ILogLoader>();

            builder.RegisterType<StatusBarViewModel>()
                .AsSelf();

            // Drag & drop
            builder.RegisterType<DropService>().SingleInstance();
            builder.RegisterType<MultiLogFileDropHandler>().SingleInstance();
            builder.RegisterType<SingleDirectoryDropHandler>().SingleInstance();
            builder.RegisterType<ArchiveFromHttpDropHandler>().SingleInstance();
            builder.RegisterType<ArchiveDropHandler>().SingleInstance();

            builder.RegisterType<VisualTransformers>()
                .As<IVisualTransformers>()
                .SingleInstance();

        }
    }
}