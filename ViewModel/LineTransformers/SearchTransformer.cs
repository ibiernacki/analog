using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using ViewModels.Panels;
using System.Text.RegularExpressions;

namespace ViewModels.LineTransformers
{
    public class SearchTransformer : DocumentColorizingTransformer, ILineTransformer
    {

        private IEnumerable<SearchCriterionViewModel> _criterions =
            Enumerable.Empty<SearchCriterionViewModel>();

        public void Update(IEnumerable<SearchCriterionViewModel> criterions)
            => _criterions = criterions;




        protected override void ColorizeLine(DocumentLine line)
        {

            var text = CurrentContext.Document.GetText(line);


            foreach (var criterion in _criterions)
            {
                Regex.Matches(text, criterion.Text, RegexOptions.IgnoreCase).Cast<Match>()
                    .ToList()
                    .ForEach(m => ChangeLinePart(line.Offset + m.Index, line.Offset + m.Index + m.Length, element => element.BackgroundBrush = new SolidColorBrush(criterion.Color)));


                //var offset = text.IndexOf(criterion.Text, StringComparison.InvariantCulture);
                
                //if (offset == -1) continue;
                //ChangeLinePart(line.Offset + offset, line.Offset + offset + criterion.Text.Length, element => element.BackgroundBrush = new SolidColorBrush(criterion.Color));

            }

        }

        public string Name { get; } = "Search";
    }


}
