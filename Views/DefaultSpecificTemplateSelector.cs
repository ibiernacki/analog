using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Views
{
    public class DefaultSpecificTemplateSelector : DataTemplateSelector
    {
        public Type SpecificDataType { get; set; }
        public DataTemplate SpecificDataTemplate { get; set; }
        public DataTemplate DefaultDataTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item?.GetType() == SpecificDataType)
            {
                return SpecificDataTemplate;
            }
            return DefaultDataTemplate;
        }
    }
}
