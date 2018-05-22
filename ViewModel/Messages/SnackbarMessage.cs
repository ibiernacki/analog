using System;
using MaterialDesignThemes.Wpf;

namespace ViewModels.Messages
{
    public class SnackbarMessage
    {
        public Action<SnackbarMessageQueue> Action { get; set; }
    }
}
