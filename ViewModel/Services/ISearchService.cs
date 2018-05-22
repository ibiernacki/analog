using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Services
{
    public interface ISearchService
    {
        void AddCriterion(string text);
    }
}
