using System;
using System.Runtime.Serialization;

namespace psdPH.Nodes
{
    [Serializable]
    public class NotCompatibleSetupException : Exception
    {
        public NotCompatibleSetupException() { }

        public NotCompatibleSetupException(string message) : base(message) { }

        public NotCompatibleSetupException(string message, Exception innerException) : base(message, innerException) { }

        protected NotCompatibleSetupException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
