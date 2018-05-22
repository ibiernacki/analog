using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ViewModels.Modules;

namespace ViewModels.DropTargets
{
    public class MultiLogFileDropHandler:IDropHandler
    {
        private readonly ILogState _logState;
        private readonly ILogLoader _logLoader;
        private readonly string[] _extensionsThatShouldBeHandledBySomeoneElse = {".zip", ".rar", ".7z"};

        public MultiLogFileDropHandler(ILogState logState, ILogLoader logLoader)
        {
            _logState = logState;
            _logLoader = logLoader;
        }
        public bool CanHandle(IDropInfo dropInfo)
        {
            return GetFilePaths(dropInfo) != null;
        }

        public async Task Handle(IDropInfo dropInfo)
        {
            var paths = GetFilePaths(dropInfo);
            await _logState.Load(await _logLoader.LoadPaths(paths));
        }

        private string[] GetFilePaths(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as IDataObject;
            var data = dataObject?.GetData(DataFormats.FileDrop, false);

            if (data == null)
            {
                return null;
            }

            var droppedPaths = (string[])data;

            var paths =  droppedPaths.Where(File.Exists).ToArray();
            if (paths.Length == 0)
            {
                return null;
            }

            if (paths.Length == 1)
            {
                var firstPath = paths[0];
                var extension = Path.GetExtension(firstPath) ?? string.Empty;
                if (_extensionsThatShouldBeHandledBySomeoneElse.Contains(extension))
                {
                    return null;
                }
            }

            return paths;
        }
    }
}
