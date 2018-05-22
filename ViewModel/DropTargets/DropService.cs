using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GongSolutions.Wpf.DragDrop;

namespace ViewModels.DropTargets
{
    public class DropService
    {
        private readonly List<IDropHandler> _dropHandlers = new List<IDropHandler>();
        public DropService(
            ArchiveFromHttpDropHandler archiveFromHttpDropHandler,
            ArchiveDropHandler archiveDropHandler,
            SingleDirectoryDropHandler singleDirectoryDropHandler, 
            MultiLogFileDropHandler multiFileDropHandler)
        {
            _dropHandlers.AddRange(new IDropHandler[]
            {
                archiveFromHttpDropHandler,
                singleDirectoryDropHandler,
                multiFileDropHandler,
                archiveDropHandler
            });
        }

        public bool CanHandle(IDropInfo dropInfo)
        {
            var canHandle =  _dropHandlers.Any(dh => dh.CanHandle(dropInfo));
            return canHandle;
        }

        public async Task Handle(IDropInfo dropInfo)
        {
            var dropHandler = _dropHandlers.FirstOrDefault(dh => dh.CanHandle(dropInfo));
            if (dropHandler == null)
            {
                return;
            }

            await dropHandler.Handle(dropInfo);
        }
    }
}
