using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekRow.xaml
    /// </summary>
    public partial class WeekRow : UserControl
    {
        public WeekRow(WeekData data)
        {
            var weekConfig = data.WeekConfig;
            InitializeComponent();
            var interDowMargin = Margin = new Thickness(2, 0, 0, 0);
            rowStack.Children.Add(new WeekTile(data, weekConfig));
            foreach (KeyValuePair<DayOfWeek, Blob> item in data.DowBlobsDict)
                rowStack.Children.Add(new DayTile(weekConfig, item.Key, data.DowBlobsDict[item.Key]) { Margin = interDowMargin });
            var renderButton = new Button()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Margin = new Thickness(5, 0, 0, 0),
                Content = "Рендер",
                Command = new RenderCommand(
                    PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument).Command, 
                CommandParameter = data
            };
            var deleteButton = new Button()
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
            rowStack.Children.Add(renderButton);
            rowStack.Children.Add(deleteButton);
        }
    }
}
