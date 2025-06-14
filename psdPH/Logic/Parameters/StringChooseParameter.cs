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
        public override Setup[] Setups { get {
                var result = new List<Setup>();
                result.Add(Setup.ComboString(getValueSetupConfig(), Strings));
                return result.ToArray();
        }
        }
        public override void Import(Parameter p) {
            base.Import(p);
            Strings =( p as StringChooseParameter).Strings;
        }
        public StringChooseParameter() { }
    }
}
