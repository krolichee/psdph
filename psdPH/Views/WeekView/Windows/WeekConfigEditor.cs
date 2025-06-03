using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Views.WeekView.Logic;
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
        PrototypeLeaf _getPrototypeByLayerName(string layername)
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

            PrototypeLeaf prototype = _getPrototypeByLayerName(pscc_w.GetResultString());

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
            ParameterConfig resultConfig(string fieldname, string desc) => new ParameterConfig(result, fieldname, desc);
            var dayTextLeafConfig = resultConfig(nameof(WeekConfig.DateTextLeafLayerName), "Текстовое поле числа дня");
            var dowTextLeafConfig = resultConfig(nameof(WeekConfig.DowTextLeafLayerName), "Текстовое поле дня недели");
            var previewTextLeafConfig = resultConfig(nameof(WeekConfig.TilePreviewTextLeafName), "Текстовое поле для предпросмотра");
            var weekDatesTextLeafConfig = resultConfig(nameof(WeekConfig.WeekDatesTextLeafName), "Текстовое поле дат недели");
            var dayDateFormatConfig = resultConfig(nameof(WeekConfig.DayDateFormat), "Формат даты дня");
            var dowFormatConfig = resultConfig(nameof(WeekConfig.DowFormat), "Формат дня недели");

            var dayDateFormats = new DateFormat[]
            {
                new NoZeroDateFormat(),new WithZeroDateFormat()
            };
            var dowFormats = new DateFormat[]
            {
                 new ShortDowFormat().Upper,
                 new ShortDowFormat().Lower,
                 new ShortDowFormat().FirstUpper,
                new FullDowFormat().Upper,
                new FullDowFormat().Lower,
                new FullDowFormat().FirstUpper,
            };
            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(Parameter.Choose(dayTextLeafConfig, textLeafs_names));
            parameters.Add(Parameter.Choose(dayDateFormatConfig, dayDateFormats));
            parameters.Add(Parameter.Choose(dowTextLeafConfig, textLeafs_names));
            parameters.Add(Parameter.Choose(dowFormatConfig, dowFormats));
            parameters.Add(Parameter.Choose(previewTextLeafConfig, textLeafs_names));
            parameters.Add(Parameter.Choose(weekDatesTextLeafConfig , root_textLeafs_names ));
            

            var conf_w = new ParametersInputWindow(parameters.ToArray(), "Настройка конфигурации недельного вида");
            if (conf_w.ShowDialog() != true)
                return false;
            return true;
        }
    }
}
