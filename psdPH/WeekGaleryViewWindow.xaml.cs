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
            if (weekGaleryConfig == null)
            {
                weekGaleryConfig = new WeekGaleryConfig();

                //Выбор прототипа
                PrototypeLeaf[] prototypes = root.getChildren<PrototypeLeaf>().Cast<PrototypeLeaf>().ToArray();
                string[] prototypes_names = prototypes.Select(l => l.LayerName).ToArray();
                StringChoiceWindow pscc_w = new StringChoiceWindow(prototypes_names.ToArray(),"Выбор прототип для дня");
                pscc_w.ShowDialog();
                PrototypeLeaf prototype = prototypes.Where(l => l.LayerName == pscc_w.getResultString()).ToArray()[0];

                //Сопоставление плейсхолдеров дням недели
                DowPlaceholderMatchWindow dwp_w = new DowPlaceholderMatchWindow(prototype);
                dwp_w.ShowDialog();

                //Превью текстовое поле
                TextLeaf[] textLeafs = root.getChildren<TextLeaf>();
                string[] textLeafs_names = textLeafs.Select(l => l.LayerName).ToArray();
                StringChoiceWindow prev_scc_w = new StringChoiceWindow(textLeafs_names, "Выбор текста для предпросмотра");
                prev_scc_w.ShowDialog();

                //Выбор текстового поля недели
                StringChoiceWindow dow_scc_w = new StringChoiceWindow(textLeafs_names, "Выбор текстового поля дня недели");
                dow_scc_w.ShowDialog();
                weekGaleryConfig.DowLayerDictionary = dwp_w.getResultDict();
                weekGaleryConfig.PrototypeName = prototype.LayerName;
                weekGaleryConfig.TilePreviewTextLeaf = prev_scc_w.getResultString();
                weekGaleryConfig.DowLabelTextLeafLayerName = dow_scc_w.getResultString();
            }
            InitializeComponent();

        }
    }
    public class WeekGaleryConfig:IParameterable
    {
        
        public Dictionary<DayOfWeek, string> DowLayerDictionary = new Dictionary<DayOfWeek, string>();
        public string TilePreviewTextLeaf;
        public string DowLabelTextLeafLayerName;
        public string PrototypeName;
        [XmlIgnore]
        public Composition Composition;
        public WeekGaleryConfig()
        {

        }
        public Parameter[] Parameters { get {
                PrototypeLeaf[] prototypes = Composition.getChildren<PrototypeLeaf>();
                var result = new List<Parameter>();
                var textLeafConfig = new ParameterConfig(this, nameof(this.Composition), "Прототип дня");
                result.Add(Parameter.Choose(textLeafConfig, prototypes));
                return result.ToArray();
            } }
    }
}
