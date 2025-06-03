using psdPH.Logic.Compositions;

namespace psdPH.Logic.Rules
{
    public class EmptyTextCondition : TextCondition
    {
        public override string ToString() => "текст пустой";
        public EmptyTextCondition(Composition composition) : base(composition) { }
        public EmptyTextCondition() : base(null) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Length==0;
        }
    }
    
}
