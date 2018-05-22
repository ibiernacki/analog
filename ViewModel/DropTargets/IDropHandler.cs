using System.Threading.Tasks;
using GongSolutions.Wpf.DragDrop;

namespace ViewModels.DropTargets
{
    public interface IDropHandler
    {
        bool CanHandle(IDropInfo dropInfo);
        Task Handle(IDropInfo dropInfo);
    }
}
