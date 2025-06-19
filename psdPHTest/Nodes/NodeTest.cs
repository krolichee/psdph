using System;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using psdPH.Logic;
using psdPH.Logic.Compositions;
using psdPH.Logic.Parameters;
using psdPH.Logic.Rules;
using psdPH.Utils.Setups;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Navigation;
using psdPH.Views.WeekView.Logic;

namespace psdPHTest.Nodes
{
    [TestCategory(TestCatagories.Automatic)]
    [TestClass]
    public class NodeTest
    {
        public void MuxNode()
        {
            var muxNode = new MuxNode();
            var fpar = new FlagParameter();
            var spar = new StringParameter("obj");
            var node = new ParameterNode(fpar);
            var fpar_setup = fpar.Setups.First(s => s.Config.FieldName == nameof(Parameter.Value));
            var spar_setup = spar.Setups.First(s => s.Config.FieldName == nameof(Parameter.Value));
            muxNode.LinkOutputTo(node, fpar_setup);
            Assert.IsTrue(muxNode.IsAppliableToOutputSetup(true));
            Assert.IsFalse(muxNode.IsAppliableToOutputSetup("111"));
        }
    }
    public interface Guided
    {
        Guid Guid { get; }
    }
    public static class GuidFinder
    {
        public static object GetObject(this Guid guid, IEnumerable<Guided> scope)
        {
            return scope.First(g => g.Guid == guid);
        }
    }
    public abstract class Node:Guided
    {
        protected event Action Applied;
        protected abstract List<Setup> Inputs { get; }
        protected abstract List<Setup> Outputs { get; }
        public abstract void Apply();
        public Guid Guid { get; protected set; }
    }
    public class ParameterNode
    {
        public ParameterNode(Parameter parameter)
        {

        }
    }
    public class NodeLink
    {
        Guid FromGuid;
        int FromSetupConfigHash;
        Guid ToGuid;
        int ToSetupConfigHash;
    }
    public class MuxNode : Node
    {
        public event Action OutputLinked;
        bool Toggle;

        protected Guid onGuid;
        protected Guid offGuid;
        public object OnObj;
        public object OffObj;

        public Setup OnSetup;
        public Setup OffSetup;
        public Setup ToggleInputSetup;

        public Setup OutputSetup;

        protected override List<Setup> Inputs => throw new NotImplementedException();

        protected override List<Setup> Outputs => throw new NotImplementedException();

        public MuxNode()
        {
            var toggleConfig = new SetupConfig(this, nameof(Toggle), "если");
            ToggleInputSetup = new CheckSetup(toggleConfig);
        }
        public override void Apply()
        {
            OutputSetup.Config.SetValue(Toggle ? OnObj : OffObj);
        }

        public bool IsAppliableToOutputSetup(object obj)
        {
            return OutputSetup.IsValidValue(obj);
        }

        internal void LinkOutputTo(ParameterNode node, Setup setup)
        {
            
        }
    }

    public class ChmoNode
    {
        private readonly ISetupable _value;
        public ChmoNode(ISetupable obj)
        {
            this._value = obj;
        }
        public Setup[] GetSetups() => _value.Setups;
        public object As(Type targetType)
        {
            if (targetType == null)
                throw new ArgumentNullException(nameof(targetType));

            // Обработка null
            if (_value == null)
            {
                if (!targetType.IsValueType || (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                    return null;

                throw new InvalidCastException($"Cannot convert null to non-nullable type {targetType.Name}");
            }

            // Прямое преобразование
            if (targetType.IsInstanceOfType(_value))
                return _value;

            // Обработка Nullable<T>
            Type underlyingType = Nullable.GetUnderlyingType(targetType);
            if (underlyingType != null)
            {
                if (_value == null) return null;
                return Convert.ChangeType(_value, underlyingType);
            }

            // Специальные обработчики для часто используемых типов
            if (targetType == typeof(bool))
                return ConvertToBool(_value);

            // Поиск операторов преобразования
            var conversionMethod = FindConversionOperator(_value.GetType(), targetType);
            if (conversionMethod != null)
                return conversionMethod.Invoke(null, new[] { _value });

            // Стандартное преобразование
            try
            {
                return Convert.ChangeType(_value, targetType);
            }
            catch (Exception ex) when (ex is InvalidCastException || ex is FormatException || ex is OverflowException)
            {
                throw new InvalidCastException(
                    $"Cannot convert from {_value.GetType().Name} to {targetType.Name}", ex);
            }
        }

        // Generic-версия как обёртка над основной
        public T As<T>()
        {
            return (T)As(typeof(T));
        }

        // Вспомогательные методы
        private static MethodInfo FindConversionOperator(Type fromType, Type toType)
        {
            return fromType.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Union(toType.GetMethods(BindingFlags.Public | BindingFlags.Static))
                .FirstOrDefault(m =>
                    (m.Name == "op_Implicit" || m.Name == "op_Explicit") &&
                    m.ReturnType == toType &&
                    m.GetParameters()[0].ParameterType == fromType);
        }
        private static bool ConvertToBool(object value)
        {
            if (value is bool b) return b;
            string str = value.ToString().Trim().ToLower();
            if (str == "true" || str == "1" || str == "yes") return true;
            if (str == "false" || str == "0" || str == "no") return false;
            throw new FormatException($"Cannot convert '{value}' to boolean");
        }
    }
}
