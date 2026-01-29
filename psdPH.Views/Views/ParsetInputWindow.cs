using psdPH.Parameters;
using psdPH.Setups;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Views
{
    public class ParsetInputWindow
    {
        SetupsInputWindow parametersWindow;
        public ParsetInputWindow(ParameterSet parset, string title = "")
        {
            List<Setup> setups = new List<Setup>();
            foreach (var parameterSetups in parset.AsCollection().Select(p => p.Setups))
            {
                setups.AddRange(parameterSetups);
                setups.Add(new JustSeparator());
            }
            parametersWindow = new SetupsInputWindow(setups.ToArray(), title);
        }
        public bool? ShowDialog()=> parametersWindow.ShowDialog();
    }
}

