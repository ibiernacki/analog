using System.Collections.Generic;
using Models;

namespace ViewModels.Services
{
    public interface IFoldingService
    {
        IEnumerable<Folding> Update(LogResult logResult);
    }
}

