using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Utils
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Delegate, Inherited = false)]
    [ComVisible(true)]
    public sealed class UpcomingAttribute : Attribute
    {
        private string _message;

        private bool _error;

        //
        // Сводка:
        //     Возвращает сообщение об обходном пути, содержащее описание альтернативных элементов
        //     программы.
        //
        // Возврат:
        //     Текстовая строка с описанием обходного пути.

        public string Message
        {
            get
            {
                return _message;
            }
        }

        //
        // Сводка:
        //     Возвращает логическое значение, позволяющее определить, будет ли компилятор считать
        //     использование устаревшего элемента программы ошибкой.
        //
        // Возврат:
        //     Значение true, если использование устаревшего элемента считается ошибкой; в противном
        //     случае — значение false. Значение по умолчанию — false.

        public bool IsError
        {

            get
            {
                return _error;
            }
        }

        //
        // Сводка:
        //     Инициализирует новый экземпляр класса System.ObsoleteAttribute стандартными свойствами.

        public UpcomingAttribute()
        {
            _message = null;
            _error = false;
        }

        //
        // Сводка:
        //     Инициализирует новый экземпляр класса System.ObsoleteAttribute указанным сообщением
        //     об обходном пути.
        //
        // Параметры:
        //   message:
        //     Строка текста, описывающая альтернативные обходные пути.

        public UpcomingAttribute(string message)
        {
            _message = message;
            _error = false;
        }

        //
        // Сводка:
        //     Инициализирует новый экземпляр класса System.ObsoleteAttribute сообщением об
        //     обходном пути и логическим значением, позволяющим определить, следует ли считать
        //     использование устаревшего элемента ошибкой.
        //
        // Параметры:
        //   message:
        //     Строка текста, описывающая альтернативные обходные пути.
        //
        //   error:
        //     true, если использование устаревшего элемента приводит к ошибке компилятора;
        //     false, если выдается предупреждение компилятора.

        public UpcomingAttribute(string message, bool error)
        {
            _message = message;
            _error = error;
        }
    }
}
