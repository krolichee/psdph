using psdPH.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Serialization
{
    /// <summary>
    /// \[Назначение класса\]
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
    public class GuidScope
    {
        private static List<Guided> Scope = new List<Guided>();
        public event Action GuidsLoaded;
        public void Add(Guided guided)
        {
          Scope.Add(guided);
        }
        public void Complete()
        {
            GuidsLoaded?.Invoke();
            Scope.Clear();
            GuidsLoaded = null;
        }
        public Guided GetByGuid(Guid guid)
        {
            return Scope.First(g => g.Guid == guid);
        }
        

        internal GuidScope() { }
    }
}
