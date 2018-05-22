using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Editors
{
    public class ExposeAttribute : Attribute
    {
        public Type OverridenType { get; set; }
    }
}
