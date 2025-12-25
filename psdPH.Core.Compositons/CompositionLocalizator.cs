using psdPH.Compositions;
using psdPH.Localization;
using psdPH.Logic.Compositions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psdPH.Core.Compositons
{
    [Localizator]
    public class CompositionLocalizator:Localizator
    {
        public override void RegisterLocalizations()
        {
            TypeLocalization.RegisterLocalization(
            new Dictionary<Type, string>
        {
                {typeof(RootBlob), "Документ" },
                {typeof(LayerBlob), "Поддокумент" },

                {typeof(PrototypeBlob), "Прототип" },
                { typeof(PlaceholderLeaf), "Заглушка" },

                //{typeof(ImageLeaf), "Изображение" },
                { typeof(TextLeaf), "Текст" },
                { typeof(LayerLeaf), "Слой" },
                { typeof(GroupLeaf), "Группа" },
                { typeof(AreaLeaf),"Зона" }

        });
        }
    }
}
