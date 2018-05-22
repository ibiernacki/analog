using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;

namespace ViewModels.DropTargets
{
    public class ArchiveDropHandler : IDropHandler
    {
        private string[] _archiveExtensions = {".zip", ".rar"};
        public bool CanHandle(IDropInfo dropInfo)
        {
            return GetArchiveName(dropInfo) != null;
        }

        public Task Handle(IDropInfo dropInfo)
        {
            var archiveName = GetArchiveName(dropInfo);
            return Task.FromResult(0);
        }

        private string GetArchiveName(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as IDataObject;
            var data = dataObject?.GetData(DataFormats.FileDrop, false);

            var droppedPaths = (string[])data;

            // handle single directory drop
            if (droppedPaths?.Length == 1 && Directory.Exists(droppedPaths[0]))
            {
                var firstPath = droppedPaths[0];

                var extension = Path.GetExtension(firstPath);
                if (_archiveExtensions.Contains(extension))
                {
                    return firstPath;
                }
            }

            return null;
        }
    }
}
