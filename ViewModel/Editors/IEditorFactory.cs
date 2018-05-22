using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Editors
{
    public interface IEditorFactory
    {
        IEnumerable<IEditor> Create<T>(T instance);
    }
}
