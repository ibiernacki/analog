using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using Caliburn.Micro;
using MaterialDesignColors;

namespace ViewModels.Panels
{
    public class SearchCriterionViewModel : PropertyChangedBase
    {
        public static readonly Hue[] AvailableColors;
        private Task _refreshTask;
        private CancellationTokenSource _refreshTaskCancellationTokenSource = new CancellationTokenSource();
        private static readonly Random _random = new Random();


        public SearchCriterionViewModel()
        {
            Color = AvailableColors
                .ElementAt(_random.Next(AvailableColors.Length)).Color;
        }

        public SearchCriterionViewModel(string text) :
            this()
        {
            Text = text;
        }

        public SearchCriterionViewModel(string text, Color color)
        {
            Text = text;
            Color = color;
        }

        static SearchCriterionViewModel()
        {
            var swatches = new SwatchesProvider().Swatches.ToList();
            AvailableColors = swatches.Select(sw =>
            {
                var hues = sw.PrimaryHues.ToArray();
                return new[] { hues[1], hues[2], hues[3], hues[5] };
            }).SelectMany(h => h).ToArray();
        }


        private string _text = string.Empty;

        public string Text
        {
            get { return _text; }
            set
            {
                _text = value;
                NotifyOfPropertyChange();
            }
        }

        private Color _color;

        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                NotifyOfPropertyChange();
            }
        }

        public void SetColor(Hue hue)
            => Color = hue?.Color ?? (Color = new Color() { R = 0, G = 0, B = 0, A = 25 });

        public BindableCollection<Hue> Colors { get; } = new BindableCollection<Hue>(AvailableColors);

        public void Next()
        {
            Console.WriteLine($"Next {Text}");
        }


    }
}