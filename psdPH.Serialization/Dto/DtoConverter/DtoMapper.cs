using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Serialization
{
    public abstract class DtoMapper<TEntity,TDto> where TEntity:class where TDto:class
    {
        protected DtoMapper() { }
        public void UpdateEntity(object entity, object dto)
        {
            validateEntity(entity);
            validateDto(dto);
            MapDtoToEntity(entity as TEntity, dto as TDto);
        }
        public void UpdateDto(object entity, object dto)
        {
            validateEntity(entity);
            validateDto(dto);
            MapEntityToDto(entity as TEntity, dto as TDto);
        }
        protected abstract void MapDtoToEntity(TEntity entity, TDto dto);
        protected abstract void MapEntityToDto(TEntity entity, TDto dto);
        private void validateDto(object dto)
        {
            if (!(dto is TDto))
                throw new ArgumentException();
        }

        private void validateEntity(object entity)
        {
            if (!(entity is TEntity))
                throw new ArgumentException();
        }
    }
}
