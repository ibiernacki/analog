using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Editing;

namespace Views.Behaviors
{
    public class SelectionBehavior : Behavior<TextEditor>
    {
        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register(
            "Selection", typeof(Selection), typeof(SelectionBehavior), new PropertyMetadata(default(Selection), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((SelectionBehavior)dependencyObject).Selection = dependencyPropertyChangedEventArgs.NewValue as Selection;
        }

        public Selection Selection
        {
            get { return (Selection)GetValue(SelectionProperty); }
            set { SetValue(SelectionProperty, value); }
        }


        protected override void OnAttached()
        {
            AssociatedObject.TextArea.SelectionChanged += TextAreaOnSelectionChanged;
            AssociatedObject.TextArea.Unloaded += TextAreaOnUnloaded;

        }

        private void TextAreaOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            Unregister();
        }

        private void TextAreaOnSelectionChanged(object sender, EventArgs eventArgs)
        {
            Selection = AssociatedObject.TextArea.Selection;
        }

        protected override void OnDetaching()
        {
            Unregister();
        }

        private void Unregister()
        {
            AssociatedObject.TextArea.SelectionChanged -= TextAreaOnSelectionChanged;
            AssociatedObject.TextArea.Unloaded -= TextAreaOnUnloaded;
        }
    }
}
