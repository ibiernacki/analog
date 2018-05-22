using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Views.CustomControls
{
    public class EzGrid : Grid
    {
        private static readonly GridLengthConverter GridLengthConverter = new GridLengthConverter();

        public static readonly DependencyProperty RowsProperty = DependencyProperty.Register(
            "Rows",
            typeof(string),
            typeof(EzGrid),
            new UIPropertyMetadata(string.Empty, OnRowsChanged));

        public static readonly DependencyProperty ColumnsProperty = DependencyProperty.Register(
            "Columns",
            typeof(string),
            typeof(EzGrid),
            new UIPropertyMetadata(string.Empty, OnColumnsChanged));


        public string Rows
        {
            get { return (string) GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public string Columns
        {
            get { return (string) GetValue(ColumnsProperty); }
            set { SetValue(ColumnsProperty, value); }
        }

        private static void OnColumnsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as EzGrid;

            if (string.IsNullOrEmpty(grid?.Columns))
            {
                return;
            }

            grid.ColumnDefinitions
                .Clear();

            grid.Columns
                .Split(',')
                .Select(GridLengthConverter.ConvertFromInvariantString)
                .Cast<GridLength>()
                .ToList()
                .ForEach(i => grid.ColumnDefinitions.Add(new ColumnDefinition {Width = i}));
        }

        private static void OnRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var grid = d as EzGrid;

            if (string.IsNullOrEmpty(grid?.Rows))
            {
                return;
            }

            grid.RowDefinitions
                .Clear();

            grid.Rows
                .Split(',')
                .Select(GridLengthConverter.ConvertFromInvariantString)
                .Cast<GridLength>()
                .ToList()
                .ForEach(i => grid.RowDefinitions.Add(new RowDefinition {Height = i}));
        }
    }
}