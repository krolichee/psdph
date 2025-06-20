using System;
using System.Linq;
using System.Reflection;
using psdPH.Logic;
using psdPH.Utils.Setups;

namespace psdPH.Nodes
{
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
