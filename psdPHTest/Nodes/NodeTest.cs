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

namespace psdPHTest.Nodes
{
	[TestClass]
	public class NodeTest
	{
		[TestMethod]
		public void SParNode()
		{
			var spar = new StringParameter("obj");
			spar.Value = "111";
			var parameterNode = new Node(spar);
			Assert.IsTrue(parameterNode.As<string>() == spar.Text);
		}
        [TestMethod]
        public void TextLeafNode()
        {
            var textLeaf = new TextLeaf() { LayerName = "buba" };
            var parameterNode = new Node(textLeaf);
            //Не должно вызывать ошибки
            parameterNode.As<TextLeaf>();
        }
        [TestMethod]
        public void RuleNode()
        {
            var spar = new StringParameter("obj");
            var blob = Blob.PathBlob("");
            var node = new Node(new TextAssignRule(blob));
            var textLeaf = new TextLeaf() { LayerName = "buba" };
            blob.AddChild(textLeaf);
            blob.ParameterSet.Add(spar);
            Setup[] setups = node.GetSetups();
            setups.First(s => s.Config.GetFieldOrPropertyType() == typeof(TextLeaf));
            setups.First(s => s.Config.GetFieldOrPropertyType() == typeof(StringParameter));
        }
        [TestMethod]
        public void ConditionNode()
        {

        }
    }

    public class Node
    {
        private readonly ISetupable _value;
        public Node(ISetupable obj)
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
