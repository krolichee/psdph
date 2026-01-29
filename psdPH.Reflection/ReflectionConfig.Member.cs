using System;

namespace psdPH.Reflection
{
    public partial class ReflectionConfig
    {
        class Member
        {
            readonly MemberGetter getter;
            readonly MemberSetter setter;
            readonly TypeGetter typeGetter;
            public Member(MemberGetter getter, MemberSetter setter, TypeGetter typeGetter)
            {
                this.getter = getter;
                this.setter = setter;
                this.typeGetter = typeGetter;
            }
            public object Value { get => getter(); set => setter(value); }
            public Type MemberType => typeGetter();

        }
    }
}
