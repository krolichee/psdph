using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
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
        public SimpleListData SimpleListData;
        public SimpleViewCommand(SimpleListData simpleListData)
        {
            SimpleListData = simpleListData;
        }
        protected override bool IsEditableCommand(object parameter) => true;
        protected override void EditExecuteCommand(object parameter)
        {
            var parset = (parameter as SimpleData).ParameterSet;
            new ParsetInputWindow(parset).ShowDialog();
        }
        protected override void CreateExecuteCommand(object parameter)
        {
            var listData = (SimpleListData)parameter;
            listData.New();
        }
        protected override void DeleteExecuteCommand(object parameter)
        {
            var item = (SimpleData)parameter;
            SimpleListData.Remove(item);
        }
    }
}
