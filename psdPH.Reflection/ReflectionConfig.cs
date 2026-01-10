using System;
using System.Reflection;

namespace psdPH.Reflection
{
    public partial class ReflectionConfig
    {
        readonly object obj;
        readonly string fieldName;
        Member member;
        public ReflectionConfig(object obj, string fieldName)
        {
            this.obj = obj;
            this.fieldName = fieldName;
            InitializeMember();
        }
        public string FieldName => fieldName;
        public object Obj => obj;
        public Type GetTypeOfObj()=> obj.GetType();
        public Type GetTypeOfMember() => member.MemberType;
        public void SetValue(object value) => member.Value = value;
        public object GetValue() => member.Value;
        public override bool Equals(object obj)
        {
            var config = obj as ReflectionConfig;
            if (config == null)
                return false;
            return obj == config.obj && fieldName == config.fieldName;
        }
        public override int GetHashCode()
        {
            return obj.GetHashCode() * 23 + fieldName?.GetHashCode() ?? 0;
        }
        void InitializeMember()
        {
            Type objType = GetTypeOfObj();
            FieldInfo fieldInfo = objType.GetField(fieldName);
            PropertyInfo propertyInfo = objType.GetProperty(fieldName);
            MemberSetter setter;
            MemberGetter getter;
            TypeGetter typeGetter;
            if (fieldInfo != null)
            {
                setter = (v) => fieldInfo.SetValue(obj, v);
                getter = () => 
                fieldInfo.GetValue(obj);
                typeGetter = () => fieldInfo.FieldType;
            }
            else if (propertyInfo != null)
            {
                setter = (v) => propertyInfo.SetValue(obj, v);
                getter = () => 
                propertyInfo.GetValue(obj);
                typeGetter = () => propertyInfo.PropertyType;
            }
            else
                throw new ArgumentException($"Поле или свойство с именем '{fieldName}' не найдено в объекте типа '{objType.Name}'.");

            member = new Member(getter, setter, typeGetter);
        }

        delegate void MemberSetter(object value);
        delegate object MemberGetter();
        delegate Type TypeGetter();
    }
}
