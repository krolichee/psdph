using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Logic.Parameters
{
    public class StringChooseParameter : StringParameter
    {
        public List<string> Strings = new List<string>();
        public override Setup[] Setups { get {
                var result = new List<Setup>();
                var stringChoiceConfig = new SetupConfig(this,nameof(Text), "значение");
                result.Add(Setup.ComboString(stringChoiceConfig, Strings));
                return result.ToArray();
        }
        }
        public override Parameter Clone()
        {
            var result = base.Clone() as StringChooseParameter;
            result.Strings = Strings;
            return result;
        }
        public StringChooseParameter() { }
    }
}
