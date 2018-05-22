using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Rendering;
using ViewModels;
using ViewModels.Services;

namespace Views.Behaviors
{
    public class FoldingBehavior : Behavior<TextEditor>
    {
        private FoldingManager _foldingManager;

        protected override void OnAttached()
        {
            AssociatedObject.DocumentChanged += AssociatedObjectOnDocumentChanged;
            AssociatedObject.Unloaded += AssociatedObjectOnUnloaded;
        }

        private void Unsubscribe()
        {
            AssociatedObject.DocumentChanged -= AssociatedObjectOnDocumentChanged;
            AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;
        }

        private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Unsubscribe();
        }

        protected override void OnDetaching()
        {
            Unsubscribe();
        }

        private void AssociatedObjectOnDocumentChanged(object sender, EventArgs eventArgs)
        {
            var textDocument = AssociatedObject.Document;
            
            if (textDocument == null) return;


            lock (AssociatedObject.TextArea.TextView.ElementGenerators)
            {
                _foldingManager = FoldingManager.Install(AssociatedObject.TextArea);
              //  ((ShellViewModel) AssociatedObject.DataContext).LogState.LogVisualizer = _foldingManager;
            }
        }

        private void UpdateFoldings()
        {
            if (Foldings == null)
            {
                return;
            }

            if (_foldingManager == null)
            {
                return;
            }
            _foldingManager.Clear();

            foreach (var folding in Foldings)
            {
                var foldingSection = _foldingManager.CreateFolding(folding.StartOffset, folding.EndOffset);
                
                foldingSection.Title = folding.Name;
                foldingSection.IsFolded = folding.IsFolded;
            }
        }

        public static readonly DependencyProperty FoldingsProperty = DependencyProperty.Register(
            "Foldings", typeof(IEnumerable<Folding>), typeof(FoldingBehavior), new PropertyMetadata(default(IEnumerable<Folding>), FoldingsPropertyChanged));

        private static void FoldingsPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as FoldingBehavior)?.UpdateFoldings();
        }

        public IList<Folding> Foldings
        {
            get { return (IList<Folding>)GetValue(FoldingsProperty); }
            set { SetValue(FoldingsProperty, value); }
        }

        public static readonly DependencyProperty CollapseProperty = DependencyProperty.Register(
            "Collapse", typeof(bool), typeof(FoldingBehavior), new PropertyMetadata(false, CollapsePropertyChanged));

        private static void CollapsePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as FoldingBehavior)?.UpdateFoldings();
        }

        public bool Collapse
        {
            get { return (bool)GetValue(CollapseProperty); }
            set { SetValue(CollapseProperty, value); }
        }
    }
}