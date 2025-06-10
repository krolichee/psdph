using Photoshop;
using psdPH.Logic.Rules;
using Condition = psdPH.Logic.Rules.Condition;

namespace psdPH.Logic
{
    public abstract class ConditionRule : Rule
    {
        public Condition Condition;
        protected ConditionRule(Composition composition) : base(composition)
        {
            Condition = new DummyCondition();
        }
        public override void RestoreComposition(Composition composition)
        {
            base.RestoreComposition(composition);
            Condition.RestoreComposition(composition);
        }

        abstract protected void _apply(Document doc);
        virtual protected void _else(Document doc) { }
        public override void Apply(Document doc)
        {
            if (Condition.IsValid())
                _apply(doc);
            else
                _else(doc);
        }
        public override bool IsSetUp()=> Condition.IsSetUp();
    };

}
