using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using Caliburn.Micro;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;

namespace Views.Behaviors
{
    public class ElementGeneratorsBehavior : Behavior<TextEditor>
    {
        protected override void OnAttached()
        {
            UpdateGenerators();
        }

        public static readonly DependencyProperty ElementGeneratorsProperty = DependencyProperty.Register(
            "ElementGenerators", typeof(INotifyCollectionChanged), typeof(ElementGeneratorsBehavior), new PropertyMetadata(default(IEnumerable<VisualLineElementGenerator>), ElementGeneratorsChanged));

        private static void ElementGeneratorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {

            var behavior = (ElementGeneratorsBehavior)d;

            if (DesignerProperties.GetIsInDesignMode(behavior))
            {
                return;
            }

            var observable = e.OldValue as INotifyCollectionChanged;
            if (observable != null)
            {
                observable.CollectionChanged -= behavior.ElementGeneratorsCollectionChanged;

                var collection = e.OldValue as IEnumerable<VisualLineElementGenerator>;
                if (collection != null)
                {

                    foreach (var element in collection)
                    {
                        behavior.AssociatedObject.TextArea.TextView.ElementGenerators.Remove(element);
                    }
                }
            }


            behavior.UpdateGenerators();
        }

        private void UpdateGenerators()
        {

            if (ElementGenerators == null)
            {
                return;
            }

            foreach (var gen in ElementGenerators)
            {
                AssociatedObject.TextArea.TextView.ElementGenerators.Add(gen);
            }
            var observable = ElementGenerators as INotifyCollectionChanged;
            if (observable != null)
            {
                observable.CollectionChanged += ElementGeneratorsCollectionChanged;
            }

        }

        private void ElementGeneratorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var x in notifyCollectionChangedEventArgs.NewItems.Cast<VisualLineElementGenerator>())
                    {
                        AssociatedObject.TextArea.TextView.ElementGenerators.Add(x);
                    }
                    return;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var x in notifyCollectionChangedEventArgs.OldItems.Cast<VisualLineElementGenerator>())
                    {
                        AssociatedObject.TextArea.TextView.ElementGenerators.Remove(x);
                    }
                    return;
                case NotifyCollectionChangedAction.Reset:
                    foreach (var x in notifyCollectionChangedEventArgs.OldItems.Cast<VisualLineElementGenerator>())
                    {
                        AssociatedObject.TextArea.TextView.ElementGenerators.Remove(x);
                    }
                    foreach (var x in notifyCollectionChangedEventArgs.NewItems.Cast<VisualLineElementGenerator>())
                    {
                        AssociatedObject.TextArea.TextView.ElementGenerators.Add(x);
                    }
                    return;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Move:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IEnumerable<VisualLineElementGenerator> ElementGenerators
        {
            get { return (IEnumerable<VisualLineElementGenerator>)GetValue(ElementGeneratorsProperty); }
            set { SetValue(ElementGeneratorsProperty, value); }
        }

    }
}
