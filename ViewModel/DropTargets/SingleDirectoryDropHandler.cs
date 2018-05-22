using System.IO;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ViewModels.Modules;

namespace ViewModels.DropTargets
{
    public class SingleDirectoryDropHandler :IDropHandler
    {
        private readonly ILogState _logState;
        private readonly ILogLoader _logLoader;

        public SingleDirectoryDropHandler(ILogState logState, ILogLoader logLoader)
        {
            _logState = logState;
            _logLoader = logLoader;
        }
        public bool CanHandle(IDropInfo dropInfo)
        {
            return GetDirectoryName(dropInfo) != null;
        }

        public async Task Handle(IDropInfo dropInfo)
        {
            var directoryName = GetDirectoryName(dropInfo);
            if (directoryName == null)
            {
                return;
            }
            var files = Directory.GetFiles(directoryName);
            await _logState.Load(await _logLoader.LoadPaths(files));
        }

        private string GetDirectoryName(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as IDataObject;
            var data = dataObject?.GetData(DataFormats.FileDrop, false);

            var droppedPaths = (string[]) data;

            // handle single directory drop
            if (droppedPaths?.Length == 1 && Directory.Exists(droppedPaths[0]))
            {
                return droppedPaths[0];
            }

            return null;
        }
    }
}
