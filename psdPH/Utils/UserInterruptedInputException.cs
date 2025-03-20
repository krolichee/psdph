using System;
using System.Runtime.Serialization;
using System.Windows;

namespace psdPH
{
    [Serializable]
    internal class UserInterruptedInputException : Exception
    {
        public UserInterruptedInputException()
        {
        }

        public UserInterruptedInputException(string message) : base(message)
        {
        }

        public UserInterruptedInputException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserInterruptedInputException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        public static void OnClosed(object sender, EventArgs e)
        {
            if ((sender as Window).DialogResult != true)
                throw new UserInterruptedInputException();
        }
    }
}