
using psdPH.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace psdPH.Serialization
{
    /// <summary>
    /// Абстракция, содержащая определения методов, производящие конверсию и реверсию
    /// объектов некоторого типа в DTO
    /// </summary>
    /// <remarks>
    /// <para><b>SOLID Validation Checklist:</b></para>
    /// 
    /// <para>📌 <b>SRP (Single Responsibility Principle):</b></para>
    /// <para>   • Какая единственная ответственность у этого класса? Предоставить методы конверсии и реверсии из DTO</para>
    /// <para>   • Сколько причин для изменения имеет этот класс? Изменение интерфейса конверсии</para>
    /// <para>   • Можно ли описать ответственность одним четким предложением? Да</para>
    /// 
    /// <para>📌 <b>OCP (Open-Closed Principle):</b></para>
    /// <para>   • Что может измениться в требованиях к этой функциональности? Тип входа/выхода конверсии (приемлемо)</para>
    /// <para>   • Закрыт ли класс для модификации? да. здесь нет конкретной реализации, 
    /// модификация может произойти только в случае добавления базового поведения конверсии</para>
    /// <para>   • Открыт ли класс для расширения? да. все методы абстрактны</para>
    /// <para>   • Достаточно ли абстракций для будущих изменений? предостаточно</para>
    /// 
    /// <para>📌 <b>LSP (Liskov Substitution Principle):</b></para>
    /// <para>   • Могут ли наследники этого класса использоваться вместо родителя? 
    /// да, наследники используются по интерфейсу этого класса</para>
    /// <para>   • Не усиливаются ли предусловия/ослабляются постусловия? нет (нет реализации)</para>
    /// <para>   • Сохраняются ли инварианты базового класса? --- </para>
    /// 
    /// <para>📌 <b>ISP (Interface Segregation Principle):</b></para>
    /// <para>   • Минимален ли интерфейс класса? минимален внешний и внутренний интерфейс класса</para>
    /// <para>   • Не реализует ли класс методы, которые не использует? нет </para>
    /// <para>   • Сколько клиентов используют каждый метод интерфейса? --- </para>
    /// 
    /// <para>📌 <b>DIP (Dependency Inversion Principle):</b></para>
    /// <para>   • Зависит ли класс от абстракций, а не от конкретных реализаций? --- </para>
    /// <para>   • Внедряются ли зависимости через конструктор? --- </para>
    /// <para>   • Управляются ли зависимости извне (DI контейнер)? --- </para>
    /// 
    /// <para>📌 <b>Дополнительные вопросы:</b></para>
    /// <para>   • Насколько тестируем этот класс? Тестируем через конкретные реализации </para>
    /// <para>   • Какая связность (coupling) с другими классами? 0 </para>
    /// <para>   • Какой уровень сцепления (cohesion) внутри класса? 0 </para>
    /// <para>   • Что будет, если потребуется изменить [конкретную зависимость]? --- </para>
    /// <para> ЗАПАХИ </para>
    /// <para> Используется небезопасная абстракция object. 
    /// Возможные решения: 
    /// 1. Шаблонный класс. 
    /// Плюсы: меньше шаблонного кода в реализации операций загрузки/выгрузки
    /// Минусы: сложности с регистратурой, бессмысленность в рамках абстракции
    /// Выход: инкапсуляция через шаблон. Создание DtoMapper. Композиция DtoMapper в конкретных реализациях
    /// </para>
    /// </remarks>
    ///
    public abstract class DtoConverter
    {

        public abstract Type DtoType { get; }
        public abstract Type EntityType { get; }

        //Метод получения DTO для объекта
        internal Identity GetIdentity(Dto dto,out UnknownEntityReference[] pendingReferences)
        {
            var obj = CreateEntity();
            UpdateEntity(obj, dto);
            pendingReferences = GetUnknownEntityReferences(obj,dto);
            var result = new Identity(dto.Guid,obj);
            return result;
        }
        //Метод получения объекта из DTO
        public Dto GetDto(object obj,out UnknownGuidReference[] pendingReferences)
        {
            var dto = CreateDto();
            UpdateDto(obj, dto);
            pendingReferences = GetUnknownGuidReferences(obj, dto);
            return dto;
        }

        protected virtual UnknownGuidReference[] GetUnknownGuidReferences(object obj, Dto dto) => new UnknownGuidReference[0];
        protected virtual UnknownEntityReference[] GetUnknownEntityReferences(object obj, Dto dto) => new UnknownEntityReference[0];
        //Фабричный метод сущности
        protected abstract object CreateEntity();
        //Фабричный метод DTO
        protected abstract Dto CreateDto();
        protected abstract void UpdateDto(object obj, object dto);
        protected abstract void UpdateEntity(object obj, object dto);
    }

}

