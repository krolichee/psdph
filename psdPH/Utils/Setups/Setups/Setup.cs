using psdPH.Utils.ReflectionParameter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml.Serialization;
using YourNamespace;
using static psdPH.Logic.PhotoshopDocumentExtension;

namespace psdPH.Utils.Setups
{
    public class Setup
    {
        public delegate void AcceptedEvent();
        public event AcceptedEvent Accepted;
        protected delegate void ChangedEvent();
        protected virtual event ChangedEvent changed;
        public Setup() { }
        public FrameworkElement Control;
        protected FieldFunctions _fieldFunctions;
        protected StackPanel _stack;

        protected Func<object, bool> isValidValue;
        protected Func<object> valueFunc;

        protected SetupConfig _config;
        public SetupConfig Config => _config;
        public void Accept()
        {
            _config.SetValue(valueFunc());
            Accepted?.Invoke();
        }
        public string ValueToString()
        {
            return _fieldFunctions.ConvertFunction(_config.GetValue()).ToString();
        }

        internal bool IsValidValue(object obj)
        {
            return isValidValue(obj);
        }

        protected Setup(SetupConfig config, FieldFunctions fieldFunctions = null)
        {
            if (fieldFunctions == null)
                fieldFunctions = new FieldFunctions();
            _fieldFunctions = fieldFunctions;
            _stack = new StackPanel();
            _stack.Orientation = Orientation.Horizontal;
            _stack.Children.Add(new Label() { Content = config.Desc });
            _config = config;
            _stack.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;

            if (config.AutoAccept)
                changed += () => Accept();
        }

        public StackPanel Stack => _stack;

    }
}
