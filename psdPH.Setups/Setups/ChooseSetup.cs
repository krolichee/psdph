using psdPH.Logic;
using psdPH.Setups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace psdPH.Utils.Setups
{
    public class ChooseSetup:Setup
    {
        protected override event ChangedEvent changed;
        public ChooseSetup(ReflectionConfig config, object[] options, FieldFunctions fieldFunctions = null):base(config,fieldFunctions)
        {
            if (options.Length == 0)
                throw new ArgumentException();

            var index = options.ToList().IndexOf(config.GetValue());
            var cb = new ComboBox() { ItemsSource = options.Select(_fieldFunctions.ConvertFunction) };
            cb.SelectedIndex = index;

            cb.SelectionChanged += (_, __) =>
                changed?.Invoke();

            valueFunc = () =>
            _fieldFunctions.RevertFunction(cb.SelectedItem);
            Control = cb;
            _stack.Children.Add(cb);
        }
    }
}
