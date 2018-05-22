using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ViewModels.Editors
{
    public sealed class EnumEditor : EditorBase
    {
        public override void Bind(EditorPropertyInfo editorProperty)
        {
            base.Bind(editorProperty);
            EnumValues = new ReadOnlyCollection<object>(Enum.GetValues(editorProperty.Property.PropertyType)
                .Cast<object>()
                .ToList());
        }

        public IReadOnlyCollection<object> EnumValues { get; private set; }


    }
}