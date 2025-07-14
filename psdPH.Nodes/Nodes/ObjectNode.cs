using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace psdPH.Nodes.Nodes
{
    public class ObjectNode:Node
    {
        object _obj;
        [XmlIgnore]
        public object Obj => _obj;
        public override string ToString() => Localization.LocalizationService.Localize(_obj.GetType());
        public ObjectNode(object obj):base()
        {
            _obj = obj;
        }
        protected Setup ObjectOutputSetup => Setup.Sealed(
            new ReflectionConfig(this, nameof(Obj), _obj.ToString())).
            WithType(_obj.GetType());
        public ObjectNode() { }
        [XmlIgnore]
        public override List<Setup> Inputs => new List<Setup>();
        
        [XmlIgnore]
        public override List<Setup> Outputs => new List<Setup>() { ObjectOutputSetup};

        protected override void _apply() {  }
    }
}
