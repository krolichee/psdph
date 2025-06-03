using psdPH.Logic.Compositions;

namespace psdPH.Logic.Rules
{
    public class NonEmptyTextCondition : TextCondition
    {
        public override string ToString() => "текст не пустой";
        public NonEmptyTextCondition(Composition composition) : base(composition) { }
        public NonEmptyTextCondition() : base(null) { }

        public override bool IsValid()
        {
            return TextLeaf.Text.Length > 0;
        }
    }
    
}
