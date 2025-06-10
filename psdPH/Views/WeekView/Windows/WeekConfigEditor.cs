using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Views.WeekView.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;

namespace psdPH.Views.WeekView
{
    /// <summary>
    /// Логика взаимодействия для WeekConfigEditor.xaml
    /// </summary>
    public class WeekConfigEditor
    {
        WeekConfig _result;
        Blob _root;
        public PrototypeLeaf Prototype;
        public WeekConfigEditor(Blob root)
        {
            this._root = root;
        }

        internal WeekConfig GetResultConfig()
        {
            return _result;
        }
        void ChooseDayPrototype()
        {
            PrototypeLeaf[] prototypes = _root.GetChildren<PrototypeLeaf>().ToArray();
            var prototypeConfig = new SetupConfig(this, nameof(Prototype), "Выбор прототип для дня");
            new SetupsInputWindow(Setup.Choose(prototypeConfig, prototypes)).ShowDialog();
        }
        static DateFormat[] DayDateFormats=> new DateFormat[]
            {
                new NoZeroDateFormat(),
                new WithZeroDateFormat()
            };
        static DateFormat[] DowFormats=> new DateFormat[]
            {
                 new ShortDowFormat().Upper,
                 new ShortDowFormat().Lower,
                 new ShortDowFormat().FirstUpper,
                new FullDowFormat().Upper,
                new FullDowFormat().Lower,
                new FullDowFormat().FirstUpper,
            };
        class DoesNotMatchException : System.Exception { }
        static class Matcher
        {
            public static void IsTrue(bool b)
            {
                if (!b)
                    throw new DoesNotMatchException();
            }
        }
        bool isSuitableAsDayBlob(Blob root)
        {
           return root.ParameterSet.GetByType<StringParameter>().Count()>=2;
        }
        bool isSuitableForWeekView(Blob root)
        {
            try
            {
                bool match = false;
                var prototypes = root.GetChildren<PrototypeLeaf>();
                for (global::System.Int32 i = 0; i < prototypes.Length && !match; i++)
                {
                    var prototype = prototypes[i];
                    bool belongsToPrototype(PlaceholderLeaf ph)
                        => ph.PrototypeLayerName == prototype.LayerName;
                    var phs = root.GetChildren<PlaceholderLeaf>().Where(belongsToPrototype);
                    match |= phs.Count() >= 7 && isSuitableAsDayBlob(prototype.Blob);
                }
                Matcher.IsTrue(match);
                Matcher.IsTrue(root.ParameterSet.GetByType<StringParameter>().Any());

                return true;
            }
            catch (DoesNotMatchException e)
            {
                return false;
            }
        }
        internal bool ShowDialog()
        {
            if (!isSuitableForWeekView(_root))
            {
                MessageBox.Show("Данный шаблон не подходит для создания этого вида");
                return false;
            }

            _result = new WeekConfig();
            //Выбор прототипа
            {
                ChooseDayPrototype();
                if (Prototype != null)
                    _result.PrototypeLayerName = Prototype.LayerName;
                else
                    return false;
            }

            //Сопоставление заглушек дням недели
            {
                DowPlaceholderMatchWindow dwpm_w = new DowPlaceholderMatchWindow(Prototype);
                if (dwpm_w.ShowDialog() != true)
                    return false;
                _result.DowPrototypeLayernameDict = dwpm_w.GetResultDict();
            }

            //Выбор особых параметров
            {
                string[] getStringParsNames(ParameterSet ps) => ps.GetByType<StringParameter>().Select(l => l.Name).ToArray();
                string[] getBlobStringParsNames(Blob blob) => getStringParsNames(blob.ParameterSet);

                string[] textPars_names = getBlobStringParsNames(Prototype.Blob);
                string[] rootTextPars_names = getBlobStringParsNames(_root);

                SetupConfig resultConfig(string fieldname, string desc) => new SetupConfig(_result, fieldname, desc);
                var dayTextParConfig = resultConfig(nameof(WeekConfig.DateParameterName), "Текстовое поле числа дня");
                var dowParConfig = resultConfig(nameof(WeekConfig.DowParameterName), "Текстовое поле дня недели");
                var weekDatesParConfig = resultConfig(nameof(WeekConfig.WeekDatesParameterName), "Текстовое поле дат недели");
                var dayDateFormatConfig = resultConfig(nameof(WeekConfig.DayDateFormat), "Формат даты дня");
                var dowFormatConfig = resultConfig(nameof(WeekConfig.DowFormat), "Формат дня недели");

                var dayDateFormats = DayDateFormats;
                var dowFormats = DowFormats;
                List<Setup> parameters = new List<Setup>();
                parameters.Add(Setup.Choose(weekDatesParConfig, rootTextPars_names));
                parameters.Add(Setup.Choose(dayTextParConfig, textPars_names));
                parameters.Add(Setup.Choose(dayDateFormatConfig, dayDateFormats));
                parameters.Add(Setup.Choose(dowParConfig, textPars_names));
                parameters.Add(Setup.Choose(dowFormatConfig, dowFormats));

                var conf_w = new SetupsInputWindow(parameters.ToArray(), "Настройка конфигурации недельного вида");
                if (conf_w.ShowDialog() != true)
                    return false;
            }
            return true;
        }
    }
}
