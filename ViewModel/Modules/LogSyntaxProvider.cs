using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Caliburn.Micro;
using ICSharpCode.AvalonEdit.Rendering;
using ViewModels.LineTransformers;

namespace ViewModels.Modules
{
    public class LogSyntaxProvider : ILogSyntaxProvider, IDisposable
    {
        private readonly ILogVisualizer _logVisualizer;
        private readonly BindableCollection<ILogSyntax> _syntaxCollection = new BindableCollection<ILogSyntax>();

        public LogSyntaxProvider(ILogSyntax[] defaultSyntaxImplementations, ILogVisualizer logVisualizer)
        {
            _logVisualizer = logVisualizer;
            _syntaxCollection.AddRange(defaultSyntaxImplementations);

            SyntaxCollection = _syntaxCollection;
            _syntaxCollection.CollectionChanged += SyntaxCollectionOnCollectionChanged;
        }

        private void SyntaxCollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    _logVisualizer.LineTransformers.Transformers.AddRange(e.NewItems.Cast<DocumentColorizingTransformer>());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    _logVisualizer.LineTransformers.Transformers.RemoveRange(e.OldItems.Cast<DocumentColorizingTransformer>());
                    break;
                case NotifyCollectionChangedAction.Reset:
                case NotifyCollectionChangedAction.Replace:
                    _logVisualizer.LineTransformers.Transformers.RemoveRange(SyntaxCollection);
                    _logVisualizer.LineTransformers.Transformers.AddRange(e.NewItems.Cast<DocumentColorizingTransformer>());
                    break;
            }
        }


        public IList<ILogSyntax> SyntaxCollection { get; }
        public ILogSyntax CurrentSyntax { get; }
                            
        public void Dispose()
        {
            _syntaxCollection.CollectionChanged -= SyntaxCollectionOnCollectionChanged;
        }
    }
}