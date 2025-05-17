using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using Photoshop;
using Application = Photoshop.Application;

namespace versiontest
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            dynamic psApp;
            Type psType;
            try
            {
                psType = Type.GetTypeFromProgID("Photoshop.Application");
                string guid = psType.GUID.ToString();
                if (guid.StartsWith("000"))
                {
                    MessageBox.Show("Нулевой GUID");
                }
                if (typeof(Photoshop.Application).GetType().GUID.ToString() != guid)
                {
                    MessageBox.Show("guid не совпадает 1");
                }
                try
                { var _ = Activator.CreateInstance(psType);
                    MessageBox.Show("Есть объект...");
                    psApp = _ as Application;
                    MessageBox.Show("Победа!");
                }
                catch { MessageBox.Show("Не удалось преобразовать в Application"); }
               
                
            }
            catch { MessageBox.Show("Не удалось получить Photoshop.Application"); }
            
        }
    }
}
