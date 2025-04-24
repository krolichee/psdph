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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows
{
    /// <summary>
    /// Логика взаимодействия для CEDStack.xaml
    /// </summary>
    public partial class CEDStack : UserControl
    {
        CEDStackContainerControl handler;
        public CEDStack(CEDStackContainerControl handler)
        {
            InitializeComponent();
            this.handler = handler;
        }
        void a()
        {
            
        }
    }
}
