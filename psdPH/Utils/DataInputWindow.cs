using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для DataInputWindow.xaml
    /// </summary>
    public class DataInputWindow
    {
        ParametersInputWindow parametersWindow;

        public DataInputWindow(Blob blob, Composition[] exclude = null,string title = "")
        {
            if (exclude == null)
                exclude = new Composition[0];
            var parameters = blob.Parameters.Where(p => !exclude.Contains(p.Config.Obj)).ToArray();
            parametersWindow = new ParametersInputWindow(parameters, title);
        }
        public bool? ShowDialog()
        {
            return parametersWindow.ShowDialog();
        }

    }
}
