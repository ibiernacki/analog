using System;
using System.Threading.Tasks;
using System.Windows;
using GongSolutions.Wpf.DragDrop;
using ViewModels.Modules;

namespace ViewModels.DropTargets
{
    public class ArchiveFromHttpDropHandler:IDropHandler
    {
        private readonly ILogState _logState;
        private readonly ILogLoader _logLoader;

        public ArchiveFromHttpDropHandler(ILogState logState, ILogLoader logLoader)
        {
            _logState = logState;
            _logLoader = logLoader;
        }
        public bool CanHandle(IDropInfo dropInfo)
        {
            return GetZipUri(dropInfo) != null;
        }

        public Task Handle(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        private Uri GetZipUri(IDropInfo dropInfo)
        {
            var dataObject = dropInfo.Data as IDataObject;
            var stringData = dataObject?.GetData(DataFormats.StringFormat)?.ToString();

            if (stringData?.EndsWith(".zip") != null && Uri.TryCreate(stringData, UriKind.Absolute, out Uri uri))
            {
                return uri;
            }

            return null;
        }
    }
}
