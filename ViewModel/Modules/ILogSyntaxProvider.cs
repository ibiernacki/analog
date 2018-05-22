using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.LineTransformers;

namespace ViewModels.Modules
{
    public interface ILogSyntaxProvider
    {
        IList<ILogSyntax> SyntaxCollection { get; }
        ILogSyntax CurrentSyntax { get; }
    }
}
