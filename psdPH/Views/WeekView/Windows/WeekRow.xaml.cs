﻿using psdPH.Logic.Compositions;
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
        Button getRenderButton(WeekData data) => new Button()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new Thickness(5, 0, 0, 0),
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
        public WeekRow(WeekData data)
        {
            InitializeComponent();
            var interDowMargin = Margin = new Thickness(2, 0, 0, 0);
            rowStack.Children.Add(new WeekTile(data));
            foreach (var dowParset in data.DayParsetsList)
                rowStack.Children.Add(new DayTile(dowParset) { Margin = interDowMargin });
            var renderButton = getRenderButton(data);
            var deleteButton = getDeleteButton(data);
            rowStack.Children.Add(renderButton);
            rowStack.Children.Add(deleteButton);
            Margin = new Thickness(0, 0, 0, 5);
        }
    }
}
