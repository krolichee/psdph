using System;
using System.Reflection;

namespace psdPH.Setups
{
    public class ReflectionConfig
    {
        public object Obj;
        public string FieldName;
        public string Desc;

        

        public Type GetTypeOfObj()
        {
            return Obj.GetType();
        }
        public Type GetFieldOrPropertyType()
        {
            
            if (Obj == null)
                throw new InvalidOperationException("Объект Obj не может быть null для определения типа.");

            Type objType = Obj.GetType();
            FieldInfo fieldInfo = objType.GetField(FieldName);
            PropertyInfo propertyInfo = objType.GetProperty(FieldName);

            if (fieldInfo != null)
                return fieldInfo.FieldType;
            else if (propertyInfo != null)
                return propertyInfo.PropertyType;
            else
                throw new ArgumentException($"Поле или свойство с именем '{FieldName}' не найдено в объекте типа '{objType.Name}'.");
        }
        public void SetValue(object value)
        {
            Type objType = GetTypeOfObj();
            FieldInfo fieldInfo = objType.GetField(FieldName);
            PropertyInfo propertyInfo = objType.GetProperty(FieldName);

            if (fieldInfo != null)
                fieldInfo.SetValue(Obj, value);
            else if (propertyInfo != null)
                propertyInfo.SetValue(Obj, value);
            else
                throw new ArgumentException($"Поле или свойство с именем '{FieldName}' не найдено в объекте типа '{objType.Name}'.");
        }


        public object GetValue()
        {
            Type objType = GetTypeOfObj();
            FieldInfo fieldInfo = objType.GetField(FieldName);
            PropertyInfo propertyInfo = objType.GetProperty(FieldName);

            if (fieldInfo != null)
                return fieldInfo.GetValue(Obj);
            else if (propertyInfo != null)
                return propertyInfo.GetValue(Obj);
            else
                throw new ArgumentException($"Поле или свойство с именем '{FieldName}' не найдено в объекте типа '{objType.Name}'.");
        }
        
        public ReflectionConfig(object obj, string fieldname, string desc = "")
        {
            Obj = obj;
            FieldName = fieldname;
            Desc = desc;
        }
        public override bool Equals(object obj)
        {
            var config = obj as ReflectionConfig;
            if (config == null)
                return false;
            return Obj == config.Obj && FieldName == config.FieldName;
        }
        public override int GetHashCode()
        {
            return Obj.GetHashCode() * 23 + FieldName?.GetHashCode() ?? 0;
        }
    }
}
