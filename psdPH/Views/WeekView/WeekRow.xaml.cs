using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Windows.Controls;

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
            rowStack.Children.Add(new WeekTile(data, weekConfig));
            foreach (KeyValuePair<DayOfWeek, Blob> item in data.DowBlobsDict)
                rowStack.Children.Add(new DayTile(weekConfig, item.Key, data.DowBlobsDict[item.Key]));
            var randerButton = new Button()
            {
                VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                Content = "Рендер",
                Command = new RenderCommand(
                    PhotoshopWrapper.GetPhotoshopApplication().ActiveDocument).Command, 
                CommandParameter= data
            };
            rowStack.Children.Add(randerButton);
        }
    }
}
