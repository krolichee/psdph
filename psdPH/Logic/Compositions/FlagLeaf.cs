using Photoshop;
using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace psdPH
{
    [Serializable]
    [UIName("Флаг")]
    public partial class FlagLeaf : Composition
    {
        public bool Toggle;
        public string Name;
        public override string ObjName => Name;
        [XmlIgnore]
        public override Setup[] Setups
        {
            get
            {
                var result = new List<Setup>();
                var toggleConfig = new SetupConfig(this, nameof(this.Toggle), Name);
                result.Add(Setup.Check(toggleConfig));
                return result.ToArray();
            }
        }

        public FlagLeaf(string name)
        {
            Name = name;
        }
        public FlagLeaf() : base() { }

        public override void Apply(Document doc) { }

        public override bool IsMatching(Document doc)
        {
            return true;
        }
    }
    
    
}

