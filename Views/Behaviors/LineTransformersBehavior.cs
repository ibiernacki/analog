using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Media;
using Caliburn.Micro;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using MoreLinq;
using ViewModels.Modules;

namespace Views.Behaviors
{
    public class LineTransformersBehavior : Behavior<TextEditor>
    {
        public static readonly DependencyProperty LineTransformersProperty = DependencyProperty.Register(
            "LineTransformers", typeof(IEnumerable<IVisualLineTransformer>), typeof(LineTransformersBehavior), new PropertyMetadata(default(IEnumerable<IVisualLineTransformer>), LineTransformersChanged));

        private IDisposable _collectionChangeSubscription = new CompositeDisposable();

        private static void LineTransformersChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = (LineTransformersBehavior)dependencyObject;

            behavior._collectionChangeSubscription.Dispose();
            behavior.UpdateTransformers(dependencyPropertyChangedEventArgs.NewValue as IEnumerable<IVisualLineTransformer>);
        }

        public IEnumerable<IVisualLineTransformer> LineTransformers
        {
            get { return (IEnumerable<IVisualLineTransformer>)GetValue(LineTransformersProperty); }
            set { SetValue(LineTransformersProperty, value); }
        }

        protected override void OnAttached()
        {
            UpdateTransformers(LineTransformers);
        }

        private void UpdateTransformers(IEnumerable<IVisualLineTransformer> transformers)
        {
            AssociatedObject.TextArea.TextView.LineTransformers.Clear();

            if (transformers == null)
            {
                return;
            }

            foreach (var t in transformers)
            {
                AssociatedObject.TextArea.TextView.LineTransformers.Add(t);
            }

            if (transformers is IReactiveCollection<IVisualLineTransformer> observable)
            {
                var added = observable.Added.ObserveOnDispatcher()
                    .Subscribe(items =>
                        items.ForEach(item => AssociatedObject.TextArea.TextView.LineTransformers.Add(item)));

                var removed = observable.Removed.ObserveOnDispatcher()
                    .Subscribe(items =>
                        items.ForEach(item => AssociatedObject.TextArea.TextView.LineTransformers.Remove(item)));

                _collectionChangeSubscription = new CompositeDisposable(added, removed);
            }
        }


        protected override void OnDetaching()
        {
            _collectionChangeSubscription.Dispose();
        }
    }
}
