using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psdPH
{
    class NumericTextBox:TextBox
    {
        public NumericTextBox() : base()
        {
            Text = "0";
            this.PreviewTextInput += TextBox_PreviewTextInput;
            this.LostFocus += TextBox_LostFocus;
        }
        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
            e.Handled = !int.TryParse(newText, out _);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!int.TryParse(textBox.Text, out _))
                textBox.Text = "0";
        }
        public int GetNumber()
        {
            return int.Parse(Text);
        }
    }
}
