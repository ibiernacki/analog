using System.Threading.Tasks;
using System.Windows;
using MaterialDesignThemes.Wpf;

namespace ViewModels.Services
{
    public class DialogService : IDialogService
    {
        public async Task ShowDialogAsync(object viewModel)
        {
            await DialogHost.Show(viewModel,
                (s, e) =>
                {
                    if (!(viewModel is IDialog)) return;
                    ((IDialog)viewModel).InputElement = (IInputElement)e.OriginalSource; ;
                }, (s, e) => { });
        }

        public async Task<T> ShowDialogAsync<T>(object viewModel)
        {
            var result = default(T);

            await DialogHost.Show(viewModel,
                (s, e) =>
                {
                    if (!(viewModel is IDialog)) return;
                    ((IDialog)viewModel).InputElement = (IInputElement)e.OriginalSource; ;
                }, (s, e) => { });
            return result;
        }
    }
}
