using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Editors
{
    public interface IEditor : IDisposable
    {
        string Name { get; }
        object Value { get; }

        void Bind(EditorPropertyInfo editorProperty);
    }
}
