using psdPH.Utils.CedStack;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;

namespace psdPH.Views.WeekView
{
    class WeekStackHandler : CEDStackHandler
    {
        WeekListData WeekListData;
        public WeekStackHandler(WeekListData weekListData)
        {
            WeekListData = weekListData;
            weekListData.Weeks.CollectionChanged += (_,__)=> Refresh();
        }
        protected override FrameworkElement createControl(object item)
        {
            return new WeekRow((WeekData)item);
        }

        protected override object[] getElements()
        {
            return WeekListData.Weeks.ToArray();
        }
        protected override void AddButtonAction()
        {
            new WeekCommand().CreateCommand.Execute(WeekListData);
        }
        protected override IList Items =>(IList) this.WeekListData.Weeks ;
    }
}
