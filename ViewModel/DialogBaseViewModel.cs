using System.Windows;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;

namespace ViewModels
{
    public class DialogBaseViewModel : PropertyChangedBase, IDialog
    {

        public virtual void Close(IInputElement inputElement = null)
        {
            DialogHost.CloseDialogCommand.Execute(null, inputElement ?? (this as IDialog).InputElement );
        }


        IInputElement IDialog.InputElement { get; set; }

    }
}