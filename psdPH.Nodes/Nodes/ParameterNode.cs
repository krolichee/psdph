using System.Linq;
using psdPH.Logic.Parameters;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using psdPH.Logic;
using System;
using psdPH.Logic.Compositions;
using psdPH.Logic.Serialization;
using System.Xml.Serialization;
using psdPH.Setups;
using psdPH.Parameters;

namespace psdPH.Nodes
{
    public class ParameterNode:Node
    {
        [XmlIgnore]
        public Parameter Parameter;
        [XmlIgnore]
        public override List<Setup> Inputs => Parameter.Setups.ToList();
        [XmlIgnore]
        public override List<Setup> Outputs => new List<Setup>();
        public object Dto
        {
            get => DtoConvertersRegistry.GetFor(this).GetDto(this);
            set => DtoConvertersRegistry.GetFor(this).ApplyDto(this, value);
        }
        public ParameterNode() : base()
        {
            DtoConvertersRegistry.Register<ParameterNode>(new ParameterNodeDtoConverter());
        }

        public ParameterNode(Parameter parameter):this()
        {
            Parameter = parameter;
        }

        protected override void _apply() {  }
    }
    public class ParameterNodeDto : Dto
    {
        public Guid ParameterGuid;
    }
    public class ParameterNodeDtoConverter : DtoConverter
    {
        public override void ApplyDto(object _obj, object _dto)
        {
            var obj = _obj as ParameterNode;
            var dto = _dto as ParameterNodeDto;
            GuidScope.Current.GuidsLoaded+=()=> obj.Parameter = GuidScope.Current.GetByGuid(dto.ParameterGuid) as Parameter;
        }

        public override object GetDto(object _obj)
        {
            var obj = _obj as ParameterNode;
            var dto = new ParameterNodeDto();
            dto.ParameterGuid = obj.Parameter.Guid;
            return dto;
        }
    }
}
