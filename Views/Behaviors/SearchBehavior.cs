using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Search;
using ViewModels.Services;

namespace Views.Behaviors
{
    public class SearchBehavior : Behavior<TextEditor>
    {
        protected override void OnAttached()
        {
            _searchPanel = SearchPanel.Install(AssociatedObject);
            AssociatedObject.TextChanged += AssociatedObjectOnDocumentChanged;

        }

        private void AssociatedObjectOnDocumentChanged(object sender, EventArgs eventArgs)
        {
            _searchPanel.Uninstall();
            _searchPanel = SearchPanel.Install(AssociatedObject);
        }

        private SearchPanel _searchPanel;
    }
}