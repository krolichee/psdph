
using psdPH.Logic.Compositions;

namespace psdPH.Logic.Rules
{
    public class EmptynessCondition : TextCondition
    {
        //TODO Локализация отправлена в далёкое эротическое путешествие???
        public override string ToString() => "Tекст пустой?";
        public EmptynessCondition() {
            SetupsRegistry.Register<EmptynessCondition>(new TextConditionSetupSource());
        }
        public override bool IsValid()
        {
            return Text.Length==0;
        }
    }
    
    
}
