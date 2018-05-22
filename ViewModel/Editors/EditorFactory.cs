using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Models;

namespace ViewModels.Editors
{
    public class EditorFactory : IEditorFactory
    {
        public EditorFactory()
        {
            _map = new Dictionary<Type, Func<IEditor>>()
            {
                {typeof(string), () => new StringEditor()},
                {typeof(bool), () => new BooleanEditor() },
                {typeof(Enum), () => new EnumEditor() },
                {typeof(DateTime), () => new DateTimeEditor() },
                {typeof(int), () => new IntegerEditor() },
            };
        }

        private readonly Dictionary<Type, Func<IEditor>> _map;



        public IEnumerable<IEditor> Create<T>(T instance)
        {
            var type = instance.GetType();
            var properties =
                type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    .Where(p => p.CanRead && p.CanWrite && p.GetCustomAttribute<ExposeAttribute>() != null);
            var editorProperties = properties.Select(p => new EditorPropertyInfo()
            {
                Description = p.GetCustomAttribute<DescriptionAttribute>()?.Description,
                Instance = instance,
                InstanceType = type,
                Name = p.GetCustomAttribute<DisplayNameAttribute>()?.DisplayName ?? p.Name,
                Property = p
            });

            return editorProperties
                .Select(epi =>
                {
                    var editor =
                        _map[
                            epi.Property.GetCustomAttribute<ExposeAttribute>().OverridenType ??
                            epi.Property.PropertyType]();
                    editor.Bind(epi);
                    return editor;
                });
        }
    }
}