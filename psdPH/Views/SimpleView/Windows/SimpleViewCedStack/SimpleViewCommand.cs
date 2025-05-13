using psdPH.Logic.Compositions;
using psdPH.Views.SimpleView.Logic;
using psdPH.Views.WeekView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Views.SimpleView.SimpleViewCedStack
{
    class SimpleViewCommand:CEDCommand
    {
        public SimpleListData SimpleViewList;
        public SimpleViewCommand(SimpleListData simpleViewList)
        {
            SimpleViewList = simpleViewList;
        }
        protected override bool IsEditableCommand(object parameter) => false;
        protected override void CreateExecuteCommand(object parameter)
        {
            var listData = (SimpleListData)parameter;
            listData.New();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            var blob = (Blob)parameter;
            SimpleViewList.Remove(blob);
        }
    }
}
