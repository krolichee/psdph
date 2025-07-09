using psdPH.Utils.Setups;
using System;
using System.Windows;
using System.Windows.Controls;

namespace psdPH.Setups
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

        protected Func<object, bool> isValidFunc;
        protected Func<object> valueFunc;

        protected ReflectionConfig _config;
        public ReflectionConfig Config => _config;

        bool nonSafe;
        public Type Type;
        public void Accept()
        {
            _config.SetValue(valueFunc());
            Accepted?.Invoke();
        }
        public string ValueToString()
        {
            return _fieldFunctions.ConvertFunction(_config.GetValue()).ToString();
        }

        public bool IsValidValue(object obj)
        {
            return isValidFunc(obj);
        }

        protected Setup(ReflectionConfig config, FieldFunctions fieldFunctions = null)
        {
            if (fieldFunctions == null)
                fieldFunctions = new FieldFunctions();
            _fieldFunctions = fieldFunctions;
            _stack = new StackPanel();
            _stack.Orientation = Orientation.Horizontal;
            _stack.Children.Add(new Label() { Content = config.Desc });
            _config = config;
            _stack.HorizontalAlignment = HorizontalAlignment.Stretch;
        }
        public Setup NonSafe()
        {
            nonSafe = true;
            return this;
        }
        bool isNone;
        public static Setup None => new Setup() { isNone = true };
        public bool IsNone() => isNone;
        public Setup AutoAccept()
        {
            changed += () => Accept();
            return this;
        }
        public Setup Sealed()
        {
            _sealed = true;
            return this;
        }

        public static Setup Sealed(ReflectionConfig config)
        {
            var result = new Setup(config);
            result.isValidFunc = (obj) => false;
            result.Control = new Grid();
            result._sealed = true;
            return result;
        }
        protected bool _sealed = false;
        public bool MayImport(Setup source)
        {
            if (_sealed)
                return false;
            if (source.nonSafe)
                return true;
            var thisConfigType = GetFieldOrPropertyType();
            var otherConfigType = source.GetFieldOrPropertyType();
            return thisConfigType.IsAssignableFrom(otherConfigType);
        }
        public Setup WithType(Type type)
        {
            Type = type;
            return this;
        }

        private Type GetFieldOrPropertyType()
        {
            if (Type != null)
                return Type;
            return _config.GetFieldOrPropertyType();
        }

        public static Setup TypeConstrained<T>(ReflectionConfig setupConfig)
        {
            var result = new Setup(setupConfig);
            result.isValidFunc = (obj) => obj is T;
            result.Control = new Grid();
            return result;
        }
        public override bool Equals(object obj)
        {
            var other = obj as Setup;
            if (other == null)
                return false;
            return Config.Equals(other.Config);
        }
        public override int GetHashCode()
        {
            return Config.GetHashCode();
        }
        public StackPanel Stack => _stack;

    }
}
