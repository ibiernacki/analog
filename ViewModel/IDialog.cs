using System.Windows;

namespace ViewModels
{
    public interface IDialog
    {
        IInputElement InputElement { get; set; }
    }
}