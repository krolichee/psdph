using psdPH.Utils.Setups;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic.Parameters
{
    public class StringChooseParameter : StringParameter
    {
        public ObservableCollection<string> Strings = new ObservableCollection<string>();
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                result.Add(new ComboStringSetup(getValueSetupConfig(), Strings));
                return result.ToArray();
            }
        }
        public override Parameter Clone()
        {
            var result = new StringChooseParameter() { Name = Name, Value = Value, Strings = Strings };
            return result;
        }
        public StringChooseParameter() { }
    }
}
