using System;
using System.Reflection;

namespace ViewModels.Editors
{
    public class EditorPropertyInfo
    {
        public PropertyInfo Property { get; set; }
        public object Instance { get; set; }
        public Type InstanceType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}