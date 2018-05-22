namespace ViewModels.Modules
{
    public class EditorContextMenuSeparator : IEditorContextMenuItem
    {
        public ContextItemType Type => ContextItemType.Separator;
        public object Header => string.Empty;

    }
}