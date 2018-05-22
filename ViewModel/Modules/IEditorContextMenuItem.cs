using MaterialDesignThemes.Wpf;

namespace ViewModels.Modules
{
    public interface IEditorContextMenuItem
    {
        ContextItemType Type { get; }
        object Header { get; }

    }
}