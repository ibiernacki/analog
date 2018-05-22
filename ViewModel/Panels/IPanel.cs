using Caliburn.Micro;

namespace ViewModels.Panels
{
    public interface IPanel : IScreen
    {
        bool IsExpanded { get; set; }
    }
}
