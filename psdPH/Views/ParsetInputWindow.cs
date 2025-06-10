using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                setups.Add(Setup.JustSeparator());
            }
            parametersWindow = new SetupsInputWindow(setups.ToArray(), title);
        }
        public bool? ShowDialog()=> parametersWindow.ShowDialog();
    }
}

