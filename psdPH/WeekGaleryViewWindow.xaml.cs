using psdPH.Logic;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Xml.Serialization;

namespace psdPH
{
    /// <summary>
    /// Логика взаимодействия для WeekGalery.xaml
    /// </summary>
    public partial class WeekGaleryViewWindow : Window
    {

        public WeekGaleryViewWindow(Blob root, WeekGaleryConfig weekGaleryConfig = null)
        {
            Prototype prototype;
            if (weekGaleryConfig == null)
            {

                

                //Выбор прототипа
                Prototype[] prototypes = root.getChildren<Prototype>().Cast<Prototype>().ToArray();
                string[] prototypes_names = prototypes.Select(l => l.LayerName).ToArray();
                StringChoiceWindow pscc_w = new StringChoiceWindow(prototypes_names.ToArray(), "Выбор прототип для дня");
                pscc_w.ShowDialog();
                prototype = prototypes.First(l => l.LayerName == pscc_w.getResultString());

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

                weekGaleryConfig = new WeekGaleryConfig
                {
                    DowLayerDictionary = dwpm_w.GetResultDict(),
                    PrototypeName = prototype.LayerName,
                    TilePreviewTextLeafName = prev_scc_w.getResultString(),
                    DowLabelTextLeafLayerName = dow_scc_w.getResultString()
                };
            }
            else
                prototype = weekGaleryConfig.Prototype;
            InitializeComponent();
            foreach (KeyValuePair<DayOfWeek, string> item in weekGaleryConfig.DowLayerDictionary)
            {
                daysStackPanel.Children.Add(new DayTile(weekGaleryConfig, item.Key, prototype.Blob.Clone()));
            }

        }
    }
    public class WeekGaleryConfig : IParameterable
    {
        public Dictionary<DayOfWeek, string> DowLayerDictionary = new Dictionary<DayOfWeek, string>();
        public string TilePreviewTextLeafName;
        public string DowLabelTextLeafLayerName;
        public string PrototypeName;
        [XmlIgnore]
        public Prototype Prototype;
        [XmlIgnore]
        public Composition Composition;
        public WeekGaleryConfig()
        {

        }
        public Parameter[] Parameters
        {
            get
            {
                Prototype[] prototypes = Composition.getChildren<Prototype>();
                var result = new List<Parameter>();
                var textLeafConfig = new ParameterConfig(this, nameof(this.Composition), "Прототип дня");
                result.Add(Parameter.Choose(textLeafConfig, prototypes));
                return result.ToArray();
            }
        }
    }
}
