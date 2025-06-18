using psdPH.Logic;
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
        public ChooseSetup(SetupConfig config, object[] options, FieldFunctions fieldFunctions = null):base(config)
        {
            if (options.Length == 0)
                throw new ArgumentException();

            var index = options.ToList().IndexOf(config.GetValue());
            var cb = new ComboBox() { ItemsSource = options.Select(fieldFunctions.ConvertFunction) };
            cb.SelectedIndex = index;

            cb.SelectionChanged += (_, __) =>
                changed?.Invoke();

            valueFunc = () =>
            fieldFunctions.RevertFunction(cb.SelectedItem);
            Control = cb;
            _stack.Children.Add(cb);
        }
    }
}
