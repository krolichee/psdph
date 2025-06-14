using Photoshop;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Logic.Ruleset.Rules
{
    public class TransitRule : ParameterSetRule, IParameterSetRule
    {
        public override string ToString() => "передать параметр";

        public override event SetupsChangedEvent SetupsChanged;
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var fromParConfig = new SetupConfig(this,nameof(FromParameter),"",true);
                var fromParSetup = Setup.Choose(fromParConfig, ParameterSet.Parameters.ToArray());
                fromParSetup.Accepted += () => 
                SetupsChanged?.Invoke(this);
                result.Add(fromParSetup);
                var toBlobConfig = new SetupConfig(this, nameof(ToBlob), "внутрь", true);
                var toBlobSetup = Setup.Choose(toBlobConfig, Composition.GetChildren<Blob>());
                toBlobSetup.Accepted += () => 
                SetupsChanged?.Invoke(this);
                result.Add(toBlobSetup);
                if(FromParameter!=null && ToBlob != null)
                {
                    bool isSameParameter(Parameter p) => 
                        p.GetType() == FromParameter.GetType();
                    var toParConfig = new SetupConfig(this, nameof(ToParameter), "в");
                    var sameParameters = ToBlob.ParameterSet.Parameters.Where(isSameParameter).ToArray();
                    var toParSetup = Setup.Choose(toParConfig, sameParameters);
                    result.Add(toParSetup);
                }
                return result.ToArray();
            }
        }
        public string ToBlobName;
        [XmlIgnore]
        public Blob ToBlob
        {
            protected get => Composition.GetChildren<Blob>().FirstOrDefault(t => t.LayerName == ToBlobName);
            set => ToBlobName = value?.LayerName;
        }
        public string FromParameterName;
        [XmlIgnore]
        public Parameter FromParameter
        {
            protected get
            {

                var parset = Composition.ParameterSet;
                return parset.AsCollection().FirstOrDefault(
                    p => p.Name == FromParameterName);
            }
            set
            {
                FromParameterName = value.Name;
            }
        }
        public string ToParameterName;
        [XmlIgnore]
        public Parameter ToParameter
        {
            protected get
            {
                var parset = ToBlob.ParameterSet;
                return parset.AsCollection().FirstOrDefault(p => p.Name == ToParameterName);
            }
            set
            {
                ToParameterName = value.Name;
            }
        }
        public override bool IsSetUp()
        {
            return base.IsSetUp()&& 
                ToBlobName!=null&&
                ToParameterName!=null&&
                FromParameterName!= null;
        }
        protected override void _apply(Document doc)
        {
            if (Composition!=null)
                ToBlob.ParameterSet.Set(ToParameterName,FromParameter.Value);
        }
        public TransitRule(Composition composition) : base(composition) { }
        public TransitRule() : base(null) { }
    }
}
