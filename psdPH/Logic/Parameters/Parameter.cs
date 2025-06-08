

using System.Collections.Generic;

namespace psdPH.Logic.Parameters
{
    public class Parameter:ISerializable,ISetupable
    {
        public string Name;
        protected Setup getNameSetup()
        {
            var nameConfig = new SetupConfig(this, nameof(this.Name), "имя");
            return Setup.StringInput(nameConfig);
        }
        public Setup[] Setups
        {
            get => new Setup[]{getNameSetup()};
        }
        public bool IsSetUp()=>Name != null;
    }
}
