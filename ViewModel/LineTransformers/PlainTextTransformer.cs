using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;

namespace ViewModels.LineTransformers
{
    public class PlainTextTransformer : DocumentColorizingTransformer, ILogSyntax
    {
        protected override void ColorizeLine(DocumentLine line)
        {
        }

        public string Name => "Plain text";
    }
}
