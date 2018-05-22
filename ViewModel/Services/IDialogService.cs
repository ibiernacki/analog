using System.Threading.Tasks;

namespace ViewModels.Services
{
    public interface IDialogService
    {
        Task ShowDialogAsync(object viewModel);
        Task<T> ShowDialogAsync<T>(object viewModel);
    }
}