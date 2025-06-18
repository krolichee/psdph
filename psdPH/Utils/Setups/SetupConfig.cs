using System;
using System.Reflection;

namespace psdPH.Utils.Setups
{
    public class SetupConfig
    {
        public object Obj;
        public string FieldName;
        public string Desc;

        public bool AutoAccept { get; internal set; }

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
        public SetupConfig(object obj, string fieldname, string desc, bool autoAccept=false)
        {
            this.Obj = obj;
            this.FieldName = fieldname;
            this.Desc = desc;
            AutoAccept = autoAccept;
        }
    }
}
