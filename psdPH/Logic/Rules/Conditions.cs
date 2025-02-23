using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace psdPH.Logic.Rules
{
    public abstract class Condition : IParameterable
    {
        public virtual string UIName { get; }
        public abstract Parameter[] Parameters { get; }

        public abstract bool IsValid();
    }
    public class SampleCondition : Condition
    {
        public override Parameter[] Parameters => throw new NotImplementedException();

        public override bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
    public abstract class TextCondition : Condition
    {
        public Composition Composition;
        //public ConditionRule rule;
        public string TextLeafLayerName;
        [XmlIgnore]
        public TextLeaf TextLeaf
        {
            get
            {
                return Composition.getChildren<TextLeaf>().Where(t => t.LayerName == TextLeafLayerName).ToArray()[0];
            }
            set
            {
                TextLeafLayerName = value.LayerName;
            }
        }
        public override Parameter[] Parameters
        {
            get
            {
                TextLeaf[] textLeaves = Composition.getChildren<TextLeaf>();
                var result = new List<Parameter>();
                var textLeafConfig = new ParameterConfig(this, nameof(this.TextLeaf), "поля");
                result.Add(Parameter.Choose(textLeafConfig, textLeaves));
                return result.ToArray();
            }
        }
        public TextCondition(Composition composition)
        {
            Composition = composition;
        }
    }

    public class MaxRowCountCondition : TextCondition
    {
        public override string UIName => "количество строк";

        public override Parameter[] Parameters
        {
            get
            {
                List<Parameter> result = new List<Parameter>().Concat(base.Parameters).ToList();
                var RowCountConfig = new ParameterConfig(this, nameof(this.RowCount), "превышает");
                result.Add(Parameter.IntInput(RowCountConfig));
                return result.ToArray();
            }
        }

        public int RowCount;

        public MaxRowCountCondition(Composition composition) : base(composition) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Split(@"\r".ToCharArray()).Length > RowCount;
        }
    }

    public class MaxRowLenCondition : TextCondition
    {
        public override string UIName => "максимальная длина строки\n среди строк";

        public override Parameter[] Parameters
        {
            get
            {
                List<Parameter> result = new List<Parameter>().Concat(base.Parameters).ToList();
                var RowLenghtConfig = new ParameterConfig(this, nameof(this.RowLength), "превышает");
                result.Add(Parameter.IntInput(RowLenghtConfig));
                return result.ToArray();
            }
        }

        public int RowLength;

        public MaxRowLenCondition(Composition composition) : base(composition) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Split(@"\r".ToCharArray()).Select(s => s.Length).Max() > RowLength;
        }
    }
}
