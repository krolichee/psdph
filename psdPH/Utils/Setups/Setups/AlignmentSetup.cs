using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Utils.Setups
{
    public class AlignmentSetup:Setup
    {
        public AlignmentSetup(SetupConfig config):base(config)
        {
            var aliControl = new AlignmentControl(config.GetValue() as Alignment);
            aliControl.Dimension = 30;
            Control = aliControl;
            _stack.Children.Add(aliControl);
            valueFunc = () => aliControl.GetResultAlignment();
        }
    }
}
