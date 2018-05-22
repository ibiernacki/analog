using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace ViewModels.Services
{
    public class SnackbarService
    {
        public SnackbarMessageQueue Queue { get; }

        public SnackbarService(SnackbarMessageQueue queue)
        {
            Queue = queue;
        }
    }
}
