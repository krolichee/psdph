using psdPH.Logic.Compositions;
using psdPH.Views.WeekView.Windows;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekRow.xaml
    /// </summary>
    public partial class WeekRow : UserControl
    {
        WeekData WeekData;
        List<DayTile> DayTiles = new List<DayTile>();
        Button getRenderButton(WeekData data) => new Button()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Top,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Margin = new Thickness(5, 0, 0, 0),
            Height = 25,
            Content = "Рендер",
            Command = new RenderCommand().Command,
            CommandParameter = data
        };
        Button getDeleteButton(WeekData data) => new Button()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Margin = new Thickness(10, 0, 0, 0),
            Width = 25,
            Height = 25,
            Foreground = new SolidColorBrush(Colors.Red),
            Content = "X",
            Command = new WeekCommand().DeleteCommand,
            CommandParameter = data
        };
        Button getTableInputButton(WeekData data) => new Button()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Margin = new Thickness(10, 0, 0, 0),
            Height = 25,
            Content = "Табличный вид",
            Command = new RelayCommand(openTableView)
        };
        void openTableView(object _)
        {
            new WeekViewTableInputIntepreter(WeekData).ShowDialog();
            refreshDayTiles();
        }
        void refreshDayTiles()
        {
            foreach (var item in DayTiles)
                item.RefreshPreview();
        }
        public WeekRow(WeekData data)
        {
            WeekData = data;
            InitializeComponent();
            var interDowMargin = Margin = new Thickness(2, 0, 0, 0);
            dowStack.Children.Add(new WeekTile(data));
            foreach (var dowParset in data.DayParsetsList)
            {
                var dayTile = new DayTile(dowParset) { Margin = interDowMargin };
                dowStack.Children.Add(dayTile);
                DayTiles.Add(dayTile);
            }
            var renderButton = getRenderButton(data);
            var deleteButton = getDeleteButton(data);
            var tableInputButton = getTableInputButton(data);
            toolBar.Children.Add(renderButton);
            toolBar.Children.Add(tableInputButton);
            dowStack.Children.Add(deleteButton);
            Margin = new Thickness(0, 0, 0, 5);
        }
    }
}
