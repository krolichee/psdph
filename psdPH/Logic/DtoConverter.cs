namespace psdPH.Logic.Compositions
{
    public abstract class DtoConverter
    {
        public abstract object GetDto(object _obj);
        public abstract void ApplyDto(object _obj, object _dto);
        public virtual void ExportDto(object _obj, object _dto) { }
    }

}

