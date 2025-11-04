using System.Linq;
using psdPH.Logic.Parameters;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System;
using psdPH.Logic.Compositions;
using System.Xml.Serialization;
using psdPH.Setups;
using psdPH.Parameters;
using psdPH.Localization;
using psdPH.Serialization;

namespace psdPH.Nodes
{
    public class ParameterNode : Node
    {
        public override string ToString() => $"{Parameter.Name}" +"\n"+
            $"<<{LocalizationService.Localize(Parameter.GetType())}>>";
        [XmlIgnore]
        public Parameter Parameter;
        [XmlIgnore]
        public override List<Setup> Inputs
        {
            get
            {
                var result = new List<Setup>();
                result.AddRange(Parameter.Setups);
                return result;
            }
        }
        [XmlIgnore]
        public override List<Setup> Outputs => new List<Setup>();
            
        public ParameterNode() : base() { }

        public ParameterNode(Parameter parameter):this()
        {
            Parameter = parameter;
        }
        protected override DtoConverter DtoConverter => new ParameterNodeDtoConverter();

        protected override void _apply() {  }
    }
    public class ParameterNodeDto : Dto
    {
        public Guid ParameterGuid;
    }
    public class ParameterNodeDtoConverter : DtoConverter
    {
        public override void GetEntity(object _obj, object _dto)
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
