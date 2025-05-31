using System;

namespace psdPH
{
    [System.AttributeUsage(System.AttributeTargets.All, Inherited = false, AllowMultiple = true)]
sealed class UINameAttribute : Attribute
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

