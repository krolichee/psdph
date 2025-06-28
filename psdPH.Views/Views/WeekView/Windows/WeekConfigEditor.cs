using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Setups;
using psdPH.TemplateEditor.CompositionLeafEditor.Windows.Utils;
using psdPH.Utils.Setups;
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
        Composition _root;
        public PrototypeBlob Prototype;
        public WeekConfigEditor(Composition root)
        {
            this._root = root;
        }
        public WeekConfigEditor(WeekConfig weekConfig)
        {
            _result = weekConfig;
        }

        internal WeekConfig GetResultConfig()
        {
            return _result;
        }
        ReflectionConfig resultConfig(string fieldname, string desc) => new ReflectionConfig(_result, fieldname, desc);
        Setup DayDateFormatSetup
        {
            get
            {
                var dayDateFormatConfig = resultConfig(nameof(WeekConfig.DayDateFormat), "Формат даты дня");
                return new ChooseSetup(dayDateFormatConfig, DayDateFormats);
            }
        }
        Setup DowFormatSetup
        {
            get
            {
                var dowFormatConfig = resultConfig(nameof(WeekConfig.DowFormat), "Формат дня недели");
            return new ChooseSetup(dowFormatConfig, DowFormats);
            }
        }


        public static void FormatsShowDialog(WeekConfig weekConfig)
        {
            var editor = new WeekConfigEditor(weekConfig);
            var setups = new Setup[] { editor.DayDateFormatSetup, editor.DowFormatSetup };
            var conf_w = new SetupsInputWindow(setups);
            conf_w.ShowDialog();
        }
        void ChooseDayPrototype()
        {
            PrototypeBlob[] prototypes = _root.GetChildren<PrototypeBlob>().ToArray();
            var prototypeConfig = new ReflectionConfig(this, nameof(Prototype), "Выбор прототип для дня");
            new SetupsInputWindow(new ChooseSetup(prototypeConfig, prototypes)).ShowDialog();
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
        bool isSuitableAsDayBlob(Composition root)
        {
           return root.ParameterSet.GetByType<StringParameter>().Count()>=2;
        }
        bool isSuitableForWeekView(Composition root)
        {
            try
            {
                bool match = false;
                var prototypes = root.GetChildren<PrototypeBlob>();
                for (global::System.Int32 i = 0; i < prototypes.Length && !match; i++)
                {
                    var prototype = prototypes[i];
                    bool belongsToPrototype(PlaceholderLeaf ph)
                        => ph.PrototypeBlob == prototype;
                    var phs = root.GetChildren<PlaceholderLeaf>().Where(belongsToPrototype);
                    match |= phs.Count() >= 7 && isSuitableAsDayBlob(prototype);
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
        internal bool NewConfigShowDialog()
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

                var dayStringPars = Prototype.ParameterSet.GetByType<StringParameter>();
                var rootStringPars = _root.ParameterSet.GetByType<StringParameter>();


                var dayTextParConfig = resultConfig(nameof(WeekConfig.DateParameterName), "Текстовое поле числа дня");
                var dowParConfig = resultConfig(nameof(WeekConfig.DowParameterName), "Текстовое поле дня недели");
                var weekDatesParConfig = resultConfig(nameof(WeekConfig.WeekDatesParameterName), "Текстовое поле дат недели");
                
                

                var dayDateFormats = DayDateFormats;
                var dowFormats = DowFormats;

                List<Setup> parameters = new List<Setup>();
                parameters.Add(new ChooseSetup(weekDatesParConfig, rootStringPars));
                parameters.Add(new ChooseSetup(dayTextParConfig, dayStringPars));
                parameters.Add(new ChooseSetup(dowParConfig, dayStringPars));

                parameters.Add(DayDateFormatSetup);
                parameters.Add(DowFormatSetup);

                var conf_w = new SetupsInputWindow(parameters.ToArray(), "Настройка конфигурации недельного вида");
                if (conf_w.ShowDialog() != true)
                    return false;
            }
            return true;
        }
    }
}
