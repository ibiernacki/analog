using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;
using Models;

namespace ViewModels.Services
{
    public class FileService : IFileService
    {
        public string[] OpenMany()
        {
            var dialog = new OpenFileDialog
            {
                Multiselect = true
            };

            dialog.ShowDialog();
            return dialog.FileNames;
        }

        public IEnumerable<StreamInfo> OpenMany(params string[] filePaths)
        {
            return
              filePaths.Select(
                  x =>
                      new StreamInfo
                      {
                          Name = x,
                          StreamReader = new FileStream(x, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
                      });
        }
    }
}