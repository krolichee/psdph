using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.Utils.CedStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.Views.WeekView
{
    class WeekStackHandler : CEDStackHandler
    {
        WeekListData WeekListData;
        protected override UIElement createControl(object item)
        {
            return new WeekRow((WeekData)item);
        }

        protected override object[] getElements()
        {
            return WeekListData.Weeks.ToArray();
        }
        protected override void AddButtonAction()
        {
            
        }
    }
}
