using psdPH.Utils.CedStack;
using System.Linq;
using System.Windows;

namespace psdPH.Views.WeekView
{
    class WeekStackHandler : CEDStackHandler
    {
        WeekListData WeekListData;
        public WeekStackHandler(WeekListData weekListData)
        {
            WeekListData = weekListData;
            weekListData.Variants.CollectionChanged += (_,__)=> Refresh();
        }
        protected override UIElement createControl(object item)
        {
            return new WeekRow((WeekData)item);
        }

        protected override object[] getElements()
        {
            return WeekListData.Variants.ToArray();
        }
        protected override void AddButtonAction()
        {
            new WeekCommand().CreateCommand.Execute(WeekListData);
        }
    }
}
