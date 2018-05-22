using System;
using Caliburn.Micro;
using ICSharpCode.AvalonEdit.Editing;
using MaterialDesignThemes.Wpf;
using ViewModels.Services;

namespace ViewModels
{
    public class ProgressDialogViewModel : DialogBaseViewModel
    {

        private string _message;

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                NotifyOfPropertyChange();
            }
        }
    }
}
