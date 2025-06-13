using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Views.SimpleView.Logic;
using psdPH.Views.SimpleView.SimpleViewCedStack;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace psdPH.Views.SimpleView.Windows.SimpleViewCedStack
{
    public class SimpleControl : CEDStackControl<SimpleData>
    {
        SimpleListData SimpleListData;
        SimpleData SimpleData;
        ParameterSet Parset=> SimpleData.ParameterSet;
        TextBlock previewTextBlock;
        public override ICommand DeleteCommand() => new SimpleViewCommand(SimpleListData).DeleteCommand;

        public override ICommand EditCommand() => new SimpleViewCommand(SimpleListData).EditCommand;
        
        public SimpleControl(SimpleData data, SimpleListData simpleListData)
        {
            SimpleData = data;
            SimpleListData = simpleListData;

            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            var stackElement = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch };

            // Определяем колонки: одна для текста (растягивается), вторая для кнопок (auto)
            stackElement.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
            stackElement.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });

            // Создаем контейнер для кнопок (StackPanel с горизонтальной ориентацией)
            var buttonsContainer = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(6, 0, 0, 0)
            };
            

            previewTextBlock = new TextBlock()
            {
                Margin = new Thickness(6, 6, 6, 0),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            var previewButton = new Button() { Content = previewTextBlock };

            var renderButton = getRenderButton(data);
            var deleteButton = getDeleteButton(data);

            // Добавляем кнопки в контейнер
            buttonsContainer.Children.Add(renderButton);
            buttonsContainer.Children.Add(deleteButton);

            // Добавляем элементы в Grid
            Grid.SetColumn(previewButton, 0);
            Grid.SetColumn(buttonsContainer, 1);

            stackElement.Children.Add(previewButton);
            stackElement.Children.Add(buttonsContainer);
            

            Margin = new Thickness(0, 0, 0, 5);

            
            previewButton.Click += button_Click;
            
            Content = stackElement;
            //setContextMenu(this,data);
            refreshPreview();
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            EditCommand().Execute(SimpleData);
            refreshPreview();
        }
        string getParametersText()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var par in Parset.AsCollection())
            {
                sb.Append($"{par.Name}: {Localization.LocalizeObj(par.Value)}\n");
            }
            return sb.ToString();
        }
        void refreshPreview()
        {
            previewTextBlock.Text = getParametersText();
        }
        Button getRenderButton(SimpleData data) => new Button()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Stretch,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
            Margin = new Thickness(5, 0, 0, 0),
            Content = "Рендер",
            Command = new SimpleRenderCommand().Command,
            CommandParameter = data
        };
        Button getDeleteButton(SimpleData data) => new Button()
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
            Margin = new Thickness(10, 0, 0, 0),
            Width = 25,
            Height = 25,
            Foreground = new SolidColorBrush(Colors.Red),
            Content = "X",
            Command = DeleteCommand(),
            CommandParameter = data
        };
    }
}
