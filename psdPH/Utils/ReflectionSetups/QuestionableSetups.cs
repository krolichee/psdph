using psdPH.Logic;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Utils.ReflectionSetups
{
    public class QuestionableSetups
    {
        public static List<Setup> Setups = new List<Setup>();
        public static void Ask()
        {
            if (Setups.Count != 0)
                new SetupsInputWindow(Setups.ToArray()).ShowDialog();
            Setups.Clear();
        }
    }
}
