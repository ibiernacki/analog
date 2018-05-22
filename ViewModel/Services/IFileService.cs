using System.Collections.Generic;
using System.IO;
using Models;

namespace ViewModels.Services
{
    public interface IFileService
    {
        string[] OpenMany();
        IEnumerable<StreamInfo> OpenMany(params string[] paths);
    }
}