using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace psdPH.Logic
{
    public class RuleSet
    {
        public List<Rule> rules;
    }
    public abstract class Rule
    {
        abstract public XmlElement apply(XmlElement xml);
    }
    public abstract class StructureRule:Rule { };
    public class ArrayPHRule :StructureRule
    {
        Blob blob;
        PlaceholderLeaf[] placeholders;

        public override XmlElement apply(XmlElement xml)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class Condition
    {
        public abstract bool IsValid(XmlElement xml);
    }
    public abstract class TextCondition : Condition
    {
        public TextLeaf TextLeaf;
    }
    public class RowCountCondition : TextCondition
    {
        public int RowCount;

        public override bool IsValid(XmlElement xml)
        {
            throw new NotImplementedException();
        }
    }
    public class MaxRowLenCondition : TextCondition
    {
        public int RowCount;

        public override bool IsValid(XmlElement xml)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class ConditionRule : Rule {
        Condition condition;
    };
    
    public enum ChangeMode
    {
        Rel,
        Abs
    }
    public abstract class ChangingRule : ConditionRule { };
    public class TranslateRule : ChangingRule { 
        public override XmlElement apply(XmlElement xml) { throw new NotImplementedException(); } };
    public abstract class TextRule : ChangingRule { };

    public class TextFontSizeRule : ChangingRule
    {
        public override XmlElement apply(XmlElement xml)
        {
            throw new NotImplementedException();
        }
    };
    public class TextAnchorRule : ChangingRule { 
        public override XmlElement apply(XmlElement xml) { throw new NotImplementedException(); } };



}
