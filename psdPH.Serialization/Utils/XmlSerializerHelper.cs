using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace psdPH.Serialization
{
    //TODO добавить обработку ошибок
    /// <summary>
    /// [Назначение класса]
    /// </summary>
    /// <remarks>
    /// <b>SOLID Checklist:</b>
    /// • SRP: Одна ответственность? □ | Причины изменений: ______
    /// • OCP: Закрыт для модификации/открыт для расширения? □
    /// • LSP: Наследники заменяют родителя? □  
    /// • ISP: Интерфейс минимален? □ | Методы без реализации? □
    /// • DIP: Зависит от абстракций? □ | DI через конструктор? □
    /// • Тестируемость: Легко тестировать? □ | Моки зависимостей? □
    /// </remarks>
    public class XmlSerializerHelper
    {
        public static string GetXml(object obj) {
            var type = obj.GetType();
            XmlSerializer serializer = new XmlSerializer(type,DtoTypesRegistry.DtoTypes);
            StringBuilder sb = new StringBuilder();
            StringWriter sw = new StringWriter(sb);
            serializer.Serialize(sw, obj);
            Console.WriteLine(sb);
            return sb.ToString();
        }
        public static T GetObj<T>(string xmlString) where T:class
        {
            return GetObj(xmlString,typeof(T)) as T;
        }
        public static object GetObj(string xmlString,Type type)
        {
            StringReader sr = new StringReader(xmlString);
            XmlSerializer serializer = new XmlSerializer(type, DtoTypesRegistry.DtoTypes);
            object result = serializer.Deserialize(sr);
            return result;
        }
    }
}
