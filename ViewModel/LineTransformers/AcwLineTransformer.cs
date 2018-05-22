using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Rendering;
using Models.Log;
using ViewModels.Modules;

namespace ViewModels.LineTransformers
{
    public class AcwLineTransformer : DocumentColorizingTransformer, ILogSyntax
    {
        private readonly ILogState _logState;
        public string Name => "ACW";

        public AcwLineTransformer(ILogState logState)
        {
            _logState = logState;
        }

        private void ColorizeStartingFromWord(DocumentLine line, string word, Brush foregroundColor, Brush backgroundColor = null)
        {
            var text = CurrentContext.Document.GetText(line);

            var wordStarIndex = text.IndexOf(word, StringComparison.InvariantCultureIgnoreCase);

            if (wordStarIndex == -1) return;

            ChangeLinePart(line.Offset + wordStarIndex, line.EndOffset,
                x =>
                {
                    if (backgroundColor != null)
                    {
                        x.TextRunProperties.SetBackgroundBrush(backgroundColor);
                    }
                    x.TextRunProperties.SetForegroundBrush(foregroundColor);
                });
        }

        protected override void ColorizeLine(DocumentLine line)
        {
            var text = CurrentContext.Document.GetText(line);
            var entry = _logState.FilteredLogs?.GetLogFromLine(line.LineNumber);

            if (text.IndexOf("[ERROR]", StringComparison.InvariantCultureIgnoreCase) != -1 || entry?.LogEntry.LogLevel == LogLevel.Error)
            {
                ChangeLinePart(line.Offset, line.EndOffset, x =>
                {
                    x.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Colors.Red));
                    var tf = new Typeface(x.TextRunProperties.Typeface.FontFamily, x.TextRunProperties.Typeface.Style,
                        FontWeights.Bold, x.TextRunProperties.Typeface.Stretch);
                    x.TextRunProperties.SetTypeface(tf);
                });
                return;
            }
            if (text.IndexOf("[WARNING]", StringComparison.InvariantCultureIgnoreCase) != -1 || entry?.LogEntry.LogLevel == LogLevel.Warning)
            {
                ChangeLinePart(line.Offset, line.EndOffset, x =>
                {
                    x.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Colors.Orange));
                    var tf = new Typeface(x.TextRunProperties.Typeface.FontFamily, x.TextRunProperties.Typeface.Style,
                        FontWeights.Bold, x.TextRunProperties.Typeface.Stretch);
                    x.TextRunProperties.SetTypeface(tf);
                });
                return;
            }

            var infoOffset = text.IndexOf("[INFO]", StringComparison.InvariantCultureIgnoreCase);
            if (infoOffset != -1)
            {
                ChangeLinePart(line.Offset + infoOffset, line.Offset + infoOffset + 6, x => x.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Colors.Green)));
            }

            var debugOffset = text.IndexOf("[DEBUG]", StringComparison.InvariantCultureIgnoreCase);
            if (debugOffset != -1)
            {
                ChangeLinePart(line.Offset + debugOffset, line.Offset + debugOffset + 7, x => x.TextRunProperties.SetForegroundBrush(new SolidColorBrush(Colors.Green)));
            }

            ColorizeStartingFromWord(line, "ACW Started", new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Red));
            ColorizeStartingFromWord(line, "CSDK: Avaya Client Services version", new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Red));
            ColorizeStartingFromWord(line, "Application exit", new SolidColorBrush(Colors.White), new SolidColorBrush(Colors.Red));

            ColorizeStartingFromWord(line, "Executing OnEntry method from SignedIn", new SolidColorBrush(Colors.WhiteSmoke), new SolidColorBrush(Colors.DodgerBlue));
            ColorizeStartingFromWord(line, "Executing OnExit method from SignedIn", new SolidColorBrush(Colors.WhiteSmoke), new SolidColorBrush(Colors.DodgerBlue));


            ColorizeStartingFromWord(line, "Executing OnEntry from SignedIn", new SolidColorBrush(Colors.WhiteSmoke), new SolidColorBrush(Colors.DodgerBlue));
            ColorizeStartingFromWord(line, "Executing OnExit from SignedIn", new SolidColorBrush(Colors.WhiteSmoke), new SolidColorBrush(Colors.DodgerBlue));


        }
    }
}
