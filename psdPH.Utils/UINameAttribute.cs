using System;

namespace psdPH.Utils
{
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
public sealed class UINameAttribute : Attribute
    {
        readonly string positionalString;
        public UINameAttribute(string positionalString)
        {
            this.positionalString = positionalString;
        }

        public string PositionalString
        {
            get { return positionalString; }
        }
    }
    
    
}

