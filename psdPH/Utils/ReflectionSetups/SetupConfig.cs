using System;
using System.Reflection;

namespace psdPH.Logic
{
    public class SetupConfig
    {
        public object Obj;
        public string FieldName;
        public string Desc;
        public Type GetTypeOfObj()
        {
            return Obj.GetType();
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
        public SetupConfig(object obj, string fieldname, string desc)
        {
            this.Obj = obj;
            this.FieldName = fieldname;
            this.Desc = desc;
        }
    }
}
