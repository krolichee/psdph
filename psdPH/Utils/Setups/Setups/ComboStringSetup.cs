using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH.Utils.Setups
{
    public class ComboStringSetup:Setup
    {
        protected override event ChangedEvent changed;
        public ComboStringSetup(SetupConfig config, ObservableCollection<string> strings):base(config)
        {
            var cbStack = new StackPanel() { Orientation = Orientation.Horizontal };

            var cb = new ComboBox() { ItemsSource = strings, IsEditable = true, MinWidth = 70 };

            cb.SelectionChanged += (_, __) => changed?.Invoke();


            void addExecute(object _)
            {
                var str = cb.Text;
                strings.Add(str);
            }
            void deleteExecute(object _) => strings.Remove(cb.Text);
            bool isInSelection(object _) => strings.Contains(cb.Text);
            bool isNotInSelection(object _) => !isInSelection(cb.Text);



            var addButton = new Button() { Content = "+" };
            addButton.Command = new RelayCommand(addExecute, isNotInSelection);


            var deleteButton = new Button() { Content = "-" };
            deleteButton.Command = new RelayCommand(deleteExecute, isInSelection);
            void refreshButtonsParametera(object sender, TextCompositionEventArgs e)
            {
                addButton.CommandParameter = e.Text;
                addButton.CommandParameter = e.Text;
            }
            cb.ItemsSource = strings;

            cb.PreviewTextInput += refreshButtonsParametera;

            //addButton.ToolTip = "Добавить в список";
            //ToolTipService.SetInitialShowDelay(addButton, 100);
            //ToolTipService.SetBetweenShowDelay(addButton, 500);
            //ToolTipService.SetShowDuration(addButton, 2000);

            cbStack.Children.Add(cb);
            cbStack.Children.Add(addButton);
            cbStack.Children.Add(deleteButton);

            cb.Text = config.GetValue() as string; ;
            valueFunc = () => cb.Text;

            _stack.Children.Add(cbStack);
            Control = cbStack;
        }
    }
}
