using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Utils.Setups
{
    public class DateSetup: Setup
    {
        public DateSetup(SetupConfig config):base(config)
        {
            var calendar = new DatePicker();
            var date = config.GetValue() as DateTime?;
            if (date != null)
                calendar.SelectedDate = date;
            Control = calendar;
            _stack.Children.Add(calendar);
            valueFunc = () => calendar.SelectedDate;
        }
    }
}
