using psdPH.RuleEditor;
using psdPH.TemplateEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.TemplateEditor.RuleDicts;

namespace psdPH.Views.WeekView
{
    public class WeekCommand:CEDCommand
    {
        protected override bool IsEditableCommand(object parameter) => false;
        protected override void CreateExecuteCommand(object parameter)
        {
            var weekListData = (WeekListData)parameter;
            weekListData.NewWeek();
        }
    }
}
