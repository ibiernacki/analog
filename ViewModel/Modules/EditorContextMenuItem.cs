using System;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;

namespace ViewModels.Modules
{
    public class EditorContextMenuItem : IEditorContextMenuItem
    {
        private readonly Func<Task> _taskFunc;
        public ContextItemType Type => ContextItemType.MenuItem;
        public object Header { get; }
        public async Task Action() => await _taskFunc();

        public PackIconKind? Icon { get; }

        public EditorContextMenuItem(object header, Func<Task> taskFunc, PackIconKind? icon = null)
        {
            Header = header;
            _taskFunc = taskFunc;
            Icon = icon;
        }
    }
}