using psdPH.Alignments;
using psdPH.Photoshop;
using psdPH.Reflection;
using psdPH.Setups;
using psdPH.SpecialControls;
using System;

namespace psdPH.Utils.Setups
{
    public class AlignmentSetup:Setup
    {
        public AlignmentSetup(ReflectionConfig config):base(config)
        {
            Alignment alignment = config.GetValue() as Alignment;
            var aliControl = new AlignmentControl();
            aliControl.Dimension = 30;
            Control = aliControl;
            _stack.Children.Add(aliControl);
            valueFunc = () => throw new NotImplementedException();
        }
    }
}
