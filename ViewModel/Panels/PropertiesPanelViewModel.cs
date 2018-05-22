using Caliburn.Micro;
using ViewModels.Editors;
using ViewModels.Rules;

namespace ViewModels.Panels
{
    public class PropertiesPanelViewModel : PanelBase
    {
        private readonly IEditorFactory _editorFactory;

        public PropertiesPanelViewModel(IEditorFactory editorFactory)
            : base("Properties")
        {
            _editorFactory = editorFactory;
            RuleProperties = new BindableCollection<IEditor>();
            IsExpanded = true;
        }

        public BindableCollection<IEditor> RuleProperties { get; }

        public void ShowRuleProperties(RuleViewModelBase rule)
        {
            foreach (var rp in RuleProperties)
            {
                rp.Dispose();
            }
            RuleProperties.Clear();
            RuleProperties.AddRange(_editorFactory.Create(rule));
            NotifyOfPropertyChange(nameof(RuleProperties));
        }
    }
}
