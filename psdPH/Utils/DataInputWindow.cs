using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System.Linq;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для DataInputWindow.xaml
    /// </summary>
    public class DataInputWindow
    {
        ParametersInputWindow parametersWindow;

        public DataInputWindow(Blob blob, Composition[] exclude = null, string title = "")
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
