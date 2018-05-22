using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using ViewModels;
using ViewModels.ViewModels;

namespace Views.Views
{
    /// <summary>
    /// Interaction logic for ShellView.xaml
    /// </summary>
    public partial class ShellView : MetroWindow
    {
        public ShellView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if (!(DataContext is ShellViewModel shellViewModel))
            {
                return;
            }

            // shellViewModel.Log.Document.Editor = TextEditor;
        }
    }
}
