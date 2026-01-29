using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace psdPH.Nodes.Editor
{
    public class DictionaryToMenuItemsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Dictionary<string, Action> dictionary)
            {
                var menu = new ContextMenu();

                foreach (var item in dictionary)
                {
                    var menuItem = new MenuItem
                    {
                        Header = item.Key,
                        Command = new RelayCommand((_)=>item.Value(),(_)=>true)
                    };
                    menu.Items.Add(menuItem);
                }

                return menu;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}