﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

class NumericTextBox : TextBox
{
    private readonly int? _min;
    private readonly int? _max;

    public NumericTextBox(int val, int? min = null, int? max = null) : base()
    {
        _min = min;
        _max = max;

        // Устанавливаем начальное значение Text в соответствии с ограничениями и переданным значением val
        Text = GetInitialValue(val).ToString();

        this.PreviewTextInput += TextBox_PreviewTextInput;
        this.LostFocus += TextBox_LostFocus;
    }

    private int GetInitialValue(int val)
    {
        if (_min.HasValue && _max.HasValue)
        {
            return Math.Max(_min.Value, Math.Min(_max.Value, val));
        }
        else if (_min.HasValue)
        {
            return Math.Max(_min.Value, val);
        }
        else if (_max.HasValue)
        {
            return Math.Min(_max.Value, val);
        }
        else
        {
            return val;
        }
    }

    private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

        if (!int.TryParse(newText, out int newValue))
        {
            e.Handled = true;
            return;
        }

        if ((_min.HasValue && newValue < _min.Value) || (_max.HasValue && newValue > _max.Value))
        {
            e.Handled = true;
        }
    }

    private void TextBox_LostFocus(object sender, RoutedEventArgs e)
    {
        var textBox = sender as TextBox;
        if (!int.TryParse(textBox.Text, out int currentValue))
        {
            textBox.Text = GetInitialValue(0).ToString();
            return;
        }

        if (_min.HasValue && currentValue < _min.Value)
        {
            textBox.Text = _min.Value.ToString();
        }
        else if (_max.HasValue && currentValue > _max.Value)
        {
            textBox.Text = _max.Value.ToString();
        }
    }

    public int GetNumber()
    {
        return int.Parse(Text);
    }
}