using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace psdPH.Nodes.Editor
{
    class DictionaryItemToMenuItem : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is KeyValuePair<string, Action> item)
            {
                    var menuItem = new MenuItem
                    {
                        Header = item.Key,
                        Command = new RelayCommand((_) => item.Value(), (_) => true)
                    };

                return menuItem;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
