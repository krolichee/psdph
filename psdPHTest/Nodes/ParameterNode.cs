using System.Linq;
using psdPH.Logic.Parameters;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using psdPH.Logic;
using System;
using psdPH.Logic.Compositions;
using psdPH.Logic.Serialization;

namespace psdPH.Nodes
{
    public class ParameterNode:Node
    {
        public Parameter Parameter;
        public ParameterNode(Parameter parameter)
        {
            Parameter = parameter;
        }
        public override List<Setup> Inputs => Parameter.Setups.ToList();

        public override List<Setup> Outputs => new List<Setup>();

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
            GuidScope.GuidsLoaded+=()=> obj.Parameter = GuidScope.GetByGuid(dto.ParameterGuid) as Parameter;
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
