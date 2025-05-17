using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekConfigEditor.xaml
    /// </summary>
    public class WeekConfigEditor
    {
        WeekConfig result;
        Blob root;
        public WeekConfigEditor(Blob root)
        {
            this.root = root;
        }

        internal WeekConfig GetResultConfig()
        {
            return result;
        }

        StringChoiceWindow DayPrototypeChoiceWindow()
        {
            PrototypeLeaf[] prototypes = root.getChildren<PrototypeLeaf>().ToArray();
            string[] prototypes_names = prototypes.Select(l => l.LayerName).ToArray();
            StringChoiceWindow pscc_w = new StringChoiceWindow(prototypes_names.ToArray(), "Выбор прототип для дня");
            return pscc_w;
        }
        PrototypeLeaf GetPrototypeByLayerName(string layername)
        {
            PrototypeLeaf[] prototypes = root.getChildren<PrototypeLeaf>().ToArray();
            return prototypes.First(l => l.LayerName == layername);
        }
        internal bool ShowDialog()
        {
            string[] root_textLeafs_names = root.getChildren<TextLeaf>().Select(l => l.LayerName).ToArray();
            StringChoiceWindow pscc_w = DayPrototypeChoiceWindow();
            if (pscc_w.ShowDialog() != true)
                return false;

            PrototypeLeaf prototype = GetPrototypeByLayerName(pscc_w.GetResultString());

            TextLeaf[] textLeafs = prototype.Blob.getChildren<TextLeaf>();
            string[] textLeafs_names = textLeafs.Select(l => l.LayerName).ToArray();
            

            //Сопоставление заглушек дням недели
            DowPlaceholderMatchWindow dwpm_w = new DowPlaceholderMatchWindow(prototype);
            if (dwpm_w.ShowDialog() != true)
                return false;


            result = new WeekConfig
            {
                DowPrototypeLayernameDict = dwpm_w.GetResultDict(),
                PrototypeLayerName = prototype.LayerName
            };
            ParameterConfig[] parameterConfigs_protoTextLeafName = new ParameterConfig[]
            {
                new ParameterConfig(result, nameof(result.DateTextLeafLayerName), "Текстовое поле числа дня"),
                new ParameterConfig(result, nameof(result.TimeTextLeafLayerName), "Текстовое поле времени дня"),
                new ParameterConfig(result, nameof(result.DowTextLeafLayerName), "Текстовое поле дня недели"),
                new ParameterConfig(result, nameof(result.TilePreviewTextLeafName), "Текстовое поле для предпросмотра")
            };
            List<Parameter> parameters = new List<Parameter>();
            foreach (var config in parameterConfigs_protoTextLeafName)
            {
                parameters.Add(Parameter.Choose(config, textLeafs_names));
            }
            parameters.Add(Parameter.Choose(
                new ParameterConfig(result, nameof(result.WeekDatesTextLeafName), "Текстовое поле дат недели"),
                root_textLeafs_names
                ));
            var conf_w = new ParametersInputWindow(parameters.ToArray(), "Настройка конфигурации недельного вида");
            if (conf_w.ShowDialog() != true)
                return false;
            return true;
        }
    }
}
