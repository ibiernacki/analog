using System;
using System.ComponentModel;
using System.Reflection;
using Caliburn.Micro;

namespace ViewModels.Editors
{
    public abstract class EditorBase : PropertyChangedBase, IEditor
    {
        protected EditorPropertyInfo EditorProperty { get; set; }
        public string Name { get; protected set; }


        public virtual object Value
        {
            get => EditorProperty.Property.GetValue(EditorProperty.Instance);
            set => EditorProperty.Property.SetValue(EditorProperty.Instance, value);
        }

        protected EditorBase()
        {

        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (propertyChangedEventArgs.PropertyName == EditorProperty.Property.Name)
            {
                NotifyOfPropertyChange(nameof(Value));
            }
        }

        public virtual void Bind(EditorPropertyInfo editorProperty)
        {
            EditorProperty = editorProperty;
            Name = editorProperty.Name;
            if (editorProperty.Instance is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged += OnPropertyChanged;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (EditorProperty.Instance is INotifyPropertyChanged notifyPropertyChanged)
            {
                notifyPropertyChanged.PropertyChanged -= OnPropertyChanged;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}