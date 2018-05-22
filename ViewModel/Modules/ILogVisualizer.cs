using System.Reactive.Linq;
using System.Threading.Tasks;
using Caliburn.Micro;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using Models;

namespace ViewModels.Modules
{
    public interface ILogVisualizer
    {
        TextDocument Document { get; }
        Selection Selection { get; set; }
        Task Display(LogResult result);
        IVisualTransformers LineTransformers { get; }
    }
}