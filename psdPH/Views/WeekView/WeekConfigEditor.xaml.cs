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

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekConfigEditor.xaml
    /// </summary>
    public partial class WeekConfigEditor : Window
    {
        WeekConfig result;
        public WeekConfigEditor(Blob root)
        {
            InitializeComponent();
            //Выбор прототипа
            Prototype[] prototypes = root.getChildren<Prototype>().Cast<Prototype>().ToArray();
            string[] prototypes_names = prototypes.Select(l => l.LayerName).ToArray();
            StringChoiceWindow pscc_w = new StringChoiceWindow(prototypes_names.ToArray(), "Выбор прототип для дня");
            pscc_w.ShowDialog();
            Prototype prototype = prototypes.First(l => l.LayerName == pscc_w.getResultString());

            TextLeaf[] textLeafs = prototype.Blob.getChildren<TextLeaf>();
            string[] textLeafs_names = textLeafs.Select(l => l.LayerName).ToArray();

            //Сопоставление плейсхолдеров дням недели
            DowPlaceholderMatchWindow dwpm_w = new DowPlaceholderMatchWindow(prototype);
            dwpm_w.ShowDialog();

            //Выбор текстового поля недели
            StringChoiceWindow dow_scc_w = new StringChoiceWindow(textLeafs_names, "Выбор текстового поля дня недели");
            dow_scc_w.ShowDialog();

            //Превью текстовое поле
            StringChoiceWindow prev_scc_w = new StringChoiceWindow(textLeafs_names, "Выбор текста для предпросмотра");
            prev_scc_w.ShowDialog();

            result = new WeekConfig
            {
                DowPrototypeLayernameDict = dwpm_w.GetResultDict(),
                PrototypeLayerName = prototype.LayerName,
                TilePreviewTextLeafName = prev_scc_w.getResultString(),
                DowLabelTextLeafLayerName = dow_scc_w.getResultString()
            };
        }

        internal WeekConfig GetResultConfig()
        {
            return result;
        }
    }
}
