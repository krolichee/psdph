using psdPH.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

namespace psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils
{
    /// <summary>
    /// Логика взаимодействия для StackOkWindow.xaml
    /// </summary>
    public partial class ParametersWindow : Window
    {
        bool applied = false;
        public bool Applied => applied;
        StackPanel stack;
        Parameter[] parameters;
        Parameter[] Parameters { set
            {
                parameters = value;
                foreach (var p in parameters)
                    stack.Children.Add(p.Stack);

            }
            get { return parameters; }
        }
        public ParametersWindow(Parameter[] parameters,string title="")
        {
            InitializeComponent();
            Title = title;
            stack = new StackPanel();
            Parameters = parameters;
            MainGrid.Children.Insert(0, stack);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in parameters)
                item.Accept();
            applied = true;
            Close();
        }
    }
}
